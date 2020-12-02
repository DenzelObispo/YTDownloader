using MediaToolkit;
using MediaToolkit.Model;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using VideoLibrary;

namespace WpfApp1
{
    public class Controller
    {
        public bool verifyURL(TextBox txtURL)
        {
            if (txtURL.Text != "")
            {
                if (txtURL.Text.Contains("https://www.youtube.com/watch?v="))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else return false;
        }

        public YouTubeVideo getVideo(TextBox txtURL)
        {
            var resList = new List<int>();
            using (var service = Client.For(YouTube.Default))
            {
                while (true)
                {
                    var url = txtURL.Text;
                    var videos = service.GetAllVideos(url);
                    foreach (var x in videos)
                    {
                        resList.Add(x.Resolution);
                    }
                    int maxRes = resList.Max();

                    var video = videos.Where(i => i.Resolution == maxRes).FirstOrDefault();

                    return video;
                }
            }
        }

        public void saveAudio(YouTubeVideo video)
        {
            string folder;
            folder = GetCustomExportFolder();

            string path = Path.Combine(folder, video.FullName);
            File.WriteAllBytes(path, video.GetBytes());

            var inputFile = new MediaFile { Filename = path };
            var outputFile = new MediaFile { Filename = path.Replace(".mp4","") + ".mp3" };
            using (var engine = new Engine())
            {
                engine.Convert(inputFile, outputFile);
            }
            if (File.Exists(path))
            {
                File.Delete(path);
            }

        }


        public YouTubeVideo getAudio(TextBox txtURL)
        {
            using (var service = Client.For(YouTube.Default))
            {
                while (true)
                {
                    var url = txtURL.Text;
                    var videos = service.GetAllVideos(url);
                    var video = videos.Where(i => i.AudioFormat == AudioFormat.Aac && i.AudioBitrate == 128).FirstOrDefault();
                    return video;
                }
            }
        }

        public void saveVideo(YouTubeVideo video)
        {
            string folder = GetCustomExportFolder();
            string path = Path.Combine(folder, video.FullName);
            File.WriteAllBytes(path, video.GetBytes());
        }

        private string getExportFolder()
        {
            var home = Environment.GetFolderPath(
                Environment.SpecialFolder.UserProfile);

            return Path.Combine(home, "Downloads");
        }

        private static string GetCustomExportFolder()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dialog.FileName.ToString();
            }
            else return null;
        }
    }
}
