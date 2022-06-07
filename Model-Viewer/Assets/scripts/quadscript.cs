using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;

public class quadscript : MonoBehaviour
{

    // Dicom har et "levende" dictionary som leses fra xml ved initDicom
    // slices må sorteres, og det basert på en tag, men at pixeldata lesing er en separat operasjon, derfor har vi nullpeker til pixeldata
    // dicomfile lagres slik at fil ikke må leses enda en gang når pixeldata hentes

    // member variables of quadScript, accessible from any function
    Slice[] _slices = null;
    int _numSlices;
    int _minIntensity;
    int _maxIntensity;
    //int _iso;
    [SerializeField]
    private Texture missingTexture;
    private bool applyToClippingPlane;
    [Header("add extra alpha to texture")]
    [Range(0f, 1f)]
    public float alphaOffset;

    public void start()
    {
        Slice.initDicom();
    }
    Slice[] processSlices(string dicomfilepath, scanPackage newScan)
    {

        string[] dicomfilenames = Directory.GetFiles(dicomfilepath, "*.dcm");
        _numSlices = dicomfilenames.Length;

        TextAsset[] textAssetArray = newScan.bytes;
        _numSlices = textAssetArray.Length;
        print("num slice files: " + _numSlices);
        Slice[] slices = new Slice[_numSlices];

        float max = -1;
        float min = 99999;
        for (int i = 0; i < _numSlices; i++)
        {
            string filename = "deprecatd";//temp fix
            TextAsset textAsset = textAssetArray[i];
            byte[] bytes = textAsset.bytes;
            slices[i] = new Slice(filename, dicomfilepath, bytes);
            SliceInfo info = slices[i].sliceInfo;
            if (info.LargestImagePixelValue > max) max = info.LargestImagePixelValue;
            if (info.SmallestImagePixelValue < min) min = info.SmallestImagePixelValue;
            // Del dataen på max før den settes inn i tekstur
            // alternativet er å dele på 2^dicombitdepth,  men det ville blitt 4096 i dette tilfelle

        }
        print("Number of slices read:" + _numSlices);
        print("Max intensity in all slices:" + max);
        print("Min intensity in all slices:" + min);

        _minIntensity = (int)min;
        _maxIntensity = (int)max;
        _minIntensity = 0;
        _maxIntensity = 2500;
        //_iso = 0;

        Array.Sort(slices);

        return slices;
    }

    internal bool isReady()
    {
        return _slices != null;
    }

    /*
loads a new DICOM package
*/
    public bool loadDicomData(string foldername, scanPackage newScan)
    {
        string dicomfilepath = Application.dataPath + @"/scans/" + foldername + @"/DICOM";
        try
        {
            print("loading DICOM data of " + foldername);
            // string dicomfilepath = Application.streamingAssetsPath +"/dicomdata/"+foldername; 
            // string dicomfilepath = Path.Combine(Application.streamingAssetsPath, "dicomdata", foldername);

            _slices = processSlices(dicomfilepath, newScan);     // loads slices from the folder above
            Texture2D texture = textureFromSlice(_slices[0]);                     // shows the first slice
            GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
            return true;
        }
        catch (FileLoadException e)
        {
            print("no DICOM file exists for " + dicomfilepath);
            _slices = new Slice[0];
            GetComponent<Renderer>().material.SetTexture("_MainTex", missingTexture);
            throw e;
        }
        /*
                List<Vector3> vertices = new List<Vector3>();
                List<int> indices = new List<int>();
                vertices.Add(new Vector3(-0.5f, -0.5f, 0));
                vertices.Add(new Vector3(0.5f, 0.5f, 0));
                indices.Add(0);
                indices.Add(1);
        */
        //  gets the mesh object and uses it to create a diagonal line
        // meshScript mscript = GameObject.Find("GameObjectMesh").GetComponent<meshScript>();
        // mscript.createMeshGeometry(vertices, indices);

    }
    public Texture2D showSlice(int n)
    {
        if (n > _slices.Length)
        {
            throw new Exception("tried to render slice not existing. n: " + n + " num slices: " + _slices.Length);
        }
        Texture2D texture = textureFromSlice(_slices[n]);
        GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
        return texture;
    }

    internal int sliceCount()
    {
        if (_slices == null)
        {
            return 0;
            throw new Exception("access slices before DICOM data is loaded");
        }
        return _slices.Length;
    }

    /*
     * get pixel at x, y in a Slice
     * returns float representing color, between 0..1
     */
    float PixelValueFromSlice(int x, int y, int z)
    {
        Slice slice = _slices[z];
        int xdim = slice.sliceInfo.Rows;
        int ydim = slice.sliceInfo.Columns;
        ushort[] pixels = slice.getPixels();
        float val = pixelval(new Vector2(x, y), xdim, pixels);
        float v = (val - _minIntensity) / _maxIntensity;      // maps [_minIntensity,_maxIntensity] to [0,1] , i.e.  _minIntensity to black and _maxIntensity to white
        return v;
    }


    Texture2D textureFromSlice(Slice slice)
    {
        int xdim = slice.sliceInfo.Rows;
        int ydim = slice.sliceInfo.Columns;

        var texture = new Texture2D(xdim, ydim, TextureFormat.RGBA32, false);     // garbage collector will tackle that it is new'ed 

        ushort[] pixels = slice.getPixels();
        float highest = float.MinValue;
        float lowest = float.MaxValue;
        for (int y = 0; y < ydim; y++)
            for (int x = 0; x < xdim; x++)
            {
                float val = pixelval(new Vector2(x, y), xdim, pixels);
                if (val > highest) highest = val;
                if (val < lowest) lowest = val;
                float v = (val - _minIntensity) / _maxIntensity;      // maps [_minIntensity,_maxIntensity] to [0,1] , i.e.  _minIntensity to black and _maxIntensity to white
                texture.SetPixel(x, y, new UnityEngine.Color(v, v, v, alpha(v)));
            }
        // print("Min and Max CT values: " + lowest + " " + highest);
        texture.filterMode = FilterMode.Point;  // nearest neigbor interpolation is used.  (alternative is FilterMode.Bilinear)
        texture.Apply();
        return texture;
    }
    private float alpha(float value)
    {
        if (value < 0.1f)
        {
            return 0;
        }
        return value + alphaOffset;
    }

    ushort pixelval(Vector2 p, int xdim, ushort[] pixels)
    {
        return pixels[(int)p.x + (int)p.y * xdim];
    }


    Vector2 vec2(float x, float y)
    {
        return new Vector2(x, y);
    }

}
