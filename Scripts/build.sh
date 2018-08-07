#! /bin/sh

project="NFC-Ball-Maze-Game"

echo "Attempting to build $project for Windows"
/Applications/Unity/Unity.app/Contents/MacOS/Unity 
  -batchmode 
  -nographics 
  -silent-crashes 
  -projectPath $(pwd) 
  -buildWindowsPlayer "$(pwd)/Build/windows/$project.exe" 
  -quit

#echo "Attempting to build $project for OS X"
#/Applications/Unity/Unity.app/Contents/MacOS/Unity 
#  -batchmode 
#  -nographics 
#  -silent-crashes 
#  -projectPath $(pwd) 
#  -buildOSXUniversalPlayer "$(pwd)/Build/osx/$project.app" 
#  -quit

#echo "Attempting to build $project for Linux"
#/Applications/Unity/Unity.app/Contents/MacOS/Unity 
#  -batchmode 
#  -nographics 
#  -silent-crashes 
#  -projectPath $(pwd) 
#  -buildLinuxUniversalPlayer "$(pwd)/Build/linux/$project.exe" 
#  -quit

echo 'Attempting to zip builds'
zip -r $(pwd)/Build/windows.zip $(pwd)/Build/windows/
#zip -r $(pwd)/Build/mac.zip $(pwd)/Build/osx/
#zip -r $(pwd)/Build/linux.zip $(pwd)/Build/linux/
