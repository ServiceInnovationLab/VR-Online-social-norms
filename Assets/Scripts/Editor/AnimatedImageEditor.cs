using UnityEngine;
using UnityEditor;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System;
using System.Linq;
using System.IO;

[CustomEditor(typeof(AnimatedImage))]
public class AnimatedImageEditor : Editor
{

    class AnimatedGif
    {
        private List<AnimatedGifFrame> mImages = new List<AnimatedGifFrame>();
        PropertyItem mTimes;
        public AnimatedGif(string path)
        {
            Image img = Image.FromFile(path);
            int frames = img.GetFrameCount(FrameDimension.Time);

            if (frames <= 1) return;

            byte[] times = img.GetPropertyItem(0x5100).Value;
            int frame = 0;
            for (; ; )
            {
                int dur = BitConverter.ToInt32(times, 4 * frame);
                mImages.Add(new AnimatedGifFrame(new Bitmap(img), dur));
                if (++frame >= frames) break;
                img.SelectActiveFrame(FrameDimension.Time, frame);
            }
            img.Dispose();
        }
        public List<AnimatedGifFrame> Images { get { return mImages; } }
    }

    class AnimatedGifFrame
    {
        internal AnimatedGifFrame(Image img, int duration)
        {
            Image = img; Duration = duration;
        }
        public Image Image { get; }
        public int Duration { get; }
    }


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Load Gif"))
        {
            var targetObject = target as AnimatedImage;

            Undo.RecordObjects(new[] { targetObject }, "Load");

            var file = EditorUtility.OpenFilePanelWithFilters("Choose image", "", new string[] { "Animated Images", "gif" });

            var data = new AnimatedGif(file);

            if (data.Images.Count > 0)
            {
                targetObject.images = new Sprite[data.Images.Count];


                var folder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(targetObject));

                var newFolder = Path.Combine(folder, targetObject.name);

                if (!Directory.Exists(newFolder))
                {
                    Directory.CreateDirectory(newFolder);
                }

                int i = 0;
                foreach (var image in data.Images)
                {
                    EditorUtility.DisplayProgressBar("Importing frames", "", (float)i / data.Images.Count);
                    var path = Path.Combine(newFolder, targetObject.name + "-" + (i) + ".png");
                    image.Image.Save(path);
                    AssetDatabase.ImportAsset(path);

                    TextureImporter tImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                    tImporter.textureType = TextureImporterType.Sprite;
                    tImporter.SaveAndReimport();

                    targetObject.images[i++] = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                }
                EditorUtility.ClearProgressBar();

                targetObject.frameTime = data.Images[0].Duration / 100.0f;

                EditorUtility.SetDirty(targetObject);
            }
        }
    }

}

