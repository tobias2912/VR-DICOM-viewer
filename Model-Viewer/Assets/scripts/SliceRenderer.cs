public interface SliceRenderer
{
    void changeActiveImages(scanPackage newScan);
    void hideImage();
    void showImage();
    int renderImage(float percent);
}