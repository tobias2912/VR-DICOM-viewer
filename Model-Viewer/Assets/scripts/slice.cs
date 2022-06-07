// DICOM Library is "Fellow Oak Dicom" from https://github.com/fo-dicom/fo-dicom

// todo: cast dicom image directly to unity 2d texture: https://github.com/fo-dicom/fo-dicom/wiki/Image-rendering

// fo-dicom in unity does not support jpeg encoded files:  https://github.com/fo-dicom/fo-dicom/wiki/Unity


using System;
using System.Collections;
using System.Collections.Generic;

using FellowOakDicom;
using System.Linq;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Render;
using FellowOakDicom.IO.Buffer;
using UnityEngine;
using System.IO;
using FellowOakDicom.IO;
using FellowOakDicom.IO.Reader;
using UnityEngine.Assertions;

public class SliceInfo
{
    // member variables are named exactly after DICOM tag names
    public string PatientId;
    public float SliceThickness;       // millimeter
    public int Rows;
    public int Columns;
    public Vector2 PixelSpacing;       // millimeter
    public int BitsAllocated;
    public int BitsStored;
    public float RescaleIntercept;
    public float RescaleSlope;
    // varies from file to file within one scan
    public float SliceLocation;
    public Vector3 ImageOrientationPatient;
    public float SmallestImagePixelValue;
    public float LargestImagePixelValue;

    public SliceInfo(DicomFile dicomfile)
    {
        // PatientId = dicomfile.Dataset.GetSingleValue<string>(DicomTag.PatientID);

        Rows = dicomfile.Dataset.GetValue<int>(DicomTag.Rows, 0);       // 512
        Columns = dicomfile.Dataset.GetValue<int>(DicomTag.Columns, 0);  // 512
        BitsAllocated = dicomfile.Dataset.GetValue<int>(DicomTag.BitsAllocated, 0); // 16
        BitsStored = dicomfile.Dataset.GetValue<int>(DicomTag.BitsStored, 0); //  12
        RescaleIntercept = dicomfile.Dataset.GetValue<float>(DicomTag.RescaleIntercept, 0); //  -1024
        RescaleSlope = dicomfile.Dataset.GetValue<float>(DicomTag.RescaleSlope, 0); // 1     Hounsfield = val*slope + intercept : http://www.idlcoyote.com/fileio_tips/hounsfield.html             
        // SmallestImagePixelValue = dicomfile.Dataset.GetValue<float>(DicomTag.SmallestImagePixelValue, 0); //  20   
        // LargestImagePixelValue = dicomfile.Dataset.GetValue<float>(DicomTag.LargestImagePixelValue, 0);  // 172     
        // See http://dicom.nema.org/medical/dicom/2014c/output/chtml/part03/sect_C.7.6.2.html for tags below
        PixelSpacing.x = dicomfile.Dataset.GetValue<float>(DicomTag.PixelSpacing, 0);     // millimeter  0.296875/0.296875  (512*0.296875mm = 152mm = 15,2kvadratcm slice størrelse)
        PixelSpacing.y = dicomfile.Dataset.GetValue<float>(DicomTag.PixelSpacing, 0);     // millimeter  0.296875/0.296875  (512*0.296875mm = 152mm = 15,2kvadratcm slice størrelse)
        // image orientation // 1/0/0/0/1/0
        ImageOrientationPatient.x = dicomfile.Dataset.GetValue<float>(DicomTag.ImagePositionPatient, 0);     //-75.8515625
        ImageOrientationPatient.y = dicomfile.Dataset.GetValue<float>(DicomTag.ImagePositionPatient, 1);     //-171.8515625 
        ImageOrientationPatient.z = dicomfile.Dataset.GetValue<float>(DicomTag.ImagePositionPatient, 2);     //332.7
        SliceThickness = dicomfile.Dataset.GetValue<float>(DicomTag.SliceThickness, 0);      // millimeter 0.6
        SliceLocation = dicomfile.Dataset.GetValue<float>(DicomTag.SliceLocation, 0); // 332.7     
    }
}


class Slice : IComparable   // IComparable so it can be sorted by sort()
{
    public DicomFile dicomFile;
    public SliceInfo sliceInfo;
    public ushort[] slicePixels = null;

    public int CompareTo(object obj)  // for IComparable so it can be sorted by sort()
    {
        Slice otherSlice = obj as Slice;
        return sliceInfo.SliceLocation.CompareTo(otherSlice.sliceInfo.SliceLocation);
    }

    public Slice(string filename, string dicomfilepath, byte[] bytes)
    {
        String path = dicomfilepath + "\\" + filename;
        /**
        Debug.Log("trying to request " + path);
        UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Get(path);
        request.SendWebRequest();
        while (!request.isDone)
        {
        }
        byte[] data = request.downloadHandler.data;
        Debug.Log(data.Length);
        Stream stream = new MemoryStream(data);
        dicomFile = DicomFile.Open(stream);
        */
        // dicomFile = DicomFile.Open(filename);
        Stream stream = new MemoryStream(bytes);
        dicomFile = DicomFile.Open(stream);
        sliceInfo = new SliceInfo(dicomFile);
    }

    private void loadPixels()
    {
        // code found at https://groups.google.com/forum/#!topic/fo-dicom/EQtF5-7_PAU
        DicomPixelData pxd = DicomPixelData.Create(dicomFile.Dataset);
        IByteBuffer buffer = pxd.GetFrame(0);
        slicePixels = FellowOakDicom.IO.ByteConverter.ToArray<ushort>(buffer);
        //byte[] bSlicePixels = Dicom.IO.ByteConverter.ToArray<byte>(buffer);

        // alternative:
        //var header = DicomPixelData.Create(dicomFile.Dataset);
        //var pixelData = PixelDataFactory.Create(header, 0);
        //ushort[] pixels = ((GrayscalePixelDataU16)pixelData).Data;
    }

    public ushort[] getPixels()
    {
        if (slicePixels == null)
            loadPixels();
        return slicePixels;
    }

    public void releasePixels()
    {
        slicePixels = null;
    }

    public static void initDicom()
    {
        var dict = new DicomDictionary();
        dict.Load(@".\Assets\scans\DICOM Dictionary.xml", DicomDictionaryFormat.XML);
        DicomDictionary.Default = dict;
    }

}
