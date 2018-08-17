using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace panorama_wallpaper_changer
{
    class Program
    {
        public string csgoInstallPath; //Folder containing csgo.exe
        public string panoramaWallpaperStoragePath; //Folder with all 'inactive' pano wallpapers
        public string panoramaWallpaperPath; //Folder with 'active' pano wallpapers and the folder above
        public string selectedWallpaper; //Wallpaper that will replace active wallpaper
        public string activeWallpaper; //Wallpaper that will be replaced by selected wallpaper
        public string saveFile = "C:\\ProgramData\\Panorama Wallpaper Changer\\saveddata.txt";
        public string[] wallpapers;
        public int wallpaperAmount;

        static void Main(string[] args)
        {
            Program pr = new Program();

            pr.Start();
        }

        void Start()
        {
            //First check if a panorama wallpaper changer save file is found
            if (File.Exists(saveFile))
            {
                //Read save file and set variables
                using (StreamReader sr = File.OpenText(saveFile))
                {
                    wallpaperAmount = int.Parse(sr.ReadLine());
                    csgoInstallPath = sr.ReadLine();
                    panoramaWallpaperPath = sr.ReadLine();
                    panoramaWallpaperStoragePath = sr.ReadLine();
                    wallpapers = Directory.GetDirectories(panoramaWallpaperStoragePath);
                    activeWallpaper = sr.ReadLine();
                }

                //If amount of wallpapers from save file and actual amount is not equal, change it
                if(wallpaperAmount != wallpapers.Length)
                {
                    wallpaperAmount = wallpapers.Length;
                    using (StreamWriter sw = File.CreateText(saveFile))
                    {
                        sw.WriteLine(wallpaperAmount);
                        sw.WriteLine(csgoInstallPath);
                        sw.WriteLine(panoramaWallpaperPath);
                        sw.WriteLine(panoramaWallpaperStoragePath);
                        sw.WriteLine(activeWallpaper);
                    }
                }

                ChooseWallpaper();
            } else {
                //Save file was not found, so user will now do setup
                Setup();
            }
        }

        public void Setup()
        {
            Console.WriteLine("No existing save file was found. Proceeding with setup.");
            Console.WriteLine("Please insert your CSGO folder (the folder that contains 'csgo.exe') replace '\\' with '\\\'");
            string userInput = Console.ReadLine();
            if (userInput == "help") { //If user types in help instead of a path, user gets instructions
                Console.WriteLine("You can find your CSGO folder using the following steps:");
                Console.WriteLine("1. Open your Steam library.");
                Console.WriteLine("2. Right-click CSGO and choose 'Properties'.");
                Console.WriteLine("3. Open the tab 'Local Files'.");
                Console.WriteLine("4. Click 'BROWSE LOCAL FILES'.");
                Console.WriteLine("You should see the path above all the files now. Copy it and insert insert it when asked again.");
                ReturnToSetup();
            } else { //If user types in a path, that path will be used
                csgoInstallPath = userInput;
                panoramaWallpaperPath = csgoInstallPath + "csgo\\panorama\\videos\\";
                panoramaWallpaperStoragePath = panoramaWallpaperPath + "stored\\";

                wallpapers = Directory.GetDirectories(panoramaWallpaperPath);
                wallpaperAmount = wallpapers.Length;

                if (!Directory.Exists("C:\\ProgramData\\Panorama Wallpaper Changer\\"))
                {
                    //If save file directory doesn't exist, create it
                    Directory.CreateDirectory("C:\\ProgramData\\Panorama Wallpaper Changer\\");
                }

                //Write data to savefile
                using (StreamWriter sw = File.CreateText(saveFile))
                {
                    sw.WriteLine(wallpaperAmount);
                    sw.WriteLine(csgoInstallPath);
                    sw.WriteLine(panoramaWallpaperPath);
                    sw.WriteLine(panoramaWallpaperStoragePath);
                }

                Console.WriteLine("Setup complete.");
                Console.WriteLine("Press any button to continue...");
                Console.ReadKey();
                Start();
            }
        }

        //Used to loop back to setup
        public void ReturnToSetup()
        {
            Setup();
        }

        public void ChooseWallpaper()
        {
            //Get a random number within range of wallpapers
            Random r = new Random();
            int i = r.Next(0, wallpaperAmount);

            selectedWallpaper = wallpapers[i];
            Console.WriteLine(selectedWallpaper);
            if (selectedWallpaper == activeWallpaper)
            {
                ReturnToChooseWallpaper();
            } else if (selectedWallpaper == panoramaWallpaperStoragePath + "backup") {
                ReturnToChooseWallpaper();
            } else {
                if (File.Exists(selectedWallpaper + "\\nuke.webm" && File.Exists(selectedWallpaper + "\\nuke540p.webm") && File.Exists(selectedWallpaper + "\\nuke720p.webm") {
                    SetWallpaper();
                } else {
                    ReturnToChooseWallpaper();
                } 
            }
        }

        //Used to loop back to wallpaper selection
        public void ReturnToChooseWallpaper()
        {
            ChooseWallpaper();
        }

        public void SetWallpaper()
        {
            //Replace active wallpaper with new wallpaper
            File.Copy(selectedWallpaper + "\\nuke.webm", panoramaWallpaperPath + "\\nuke.webm", true);
            File.Copy(selectedWallpaper + "\\nuke540p.webm", panoramaWallpaperPath + "\\nuke540p.webm", true);
            File.Copy(selectedWallpaper + "\\nuke720p.webm", panoramaWallpaperPath + "\\nuke720p.webm", true);
            
            //Update activeWallpaper here and in save file
            activeWallpaper = selectedWallpaper;
            using (StreamWriter sw = File.CreateText(saveFile))
            {
                sw.WriteLine(wallpaperAmount);
                sw.WriteLine(csgoInstallPath);
                sw.WriteLine(panoramaWallpaperPath);
                sw.WriteLine(panoramaWallpaperStoragePath);
                sw.WriteLine(activeWallpaper);
            }

            Console.WriteLine("Wallpaper set to " + activeWallpaper);
            Console.ReadKey();
        }
    }
}
