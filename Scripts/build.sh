#! /bin/sh

project="NFC-Ball-Maze-Game"

touch $(pwd)/unity.log
echo "Attempting to build $project for Windows"
tail -F $(pwd)/unity.log & /Applications/Unity/Unity.app/Contents/MacOS/Unity 
  -batchmode 
  -nographics 
  -silent-crashes 
  -logFile $(pwd)/unity.log 
  -projectPath $(pwd) 
  -buildWindowsPlayer "$(pwd)/Build/windows/$project.exe" 
  -quit
kill %1

#touch $(pwd)/unity.log
#echo "Attempting to build $project for OS X"
#/Applications/Unity/Unity.app/Contents/MacOS/Unity 
#  -batchmode 
#  -nographics 
#  -silent-crashes 
#  -logFile $(pwd)/unity.log 
#  -projectPath $(pwd) 
#  -buildOSXUniversalPlayer "$(pwd)/Build/osx/$project.app" 
#  -quit
#kill %1

#touch $(pwd)/unity.log
#echo "Attempting to build $project for Linux"
#/Applications/Unity/Unity.app/Contents/MacOS/Unity 
#  -batchmode 
#  -nographics 
#  -silent-crashes 
#  -logFile $(pwd)/unity.log 
#  -projectPath $(pwd) 
#  -buildLinuxUniversalPlayer "$(pwd)/Build/linux/$project.exe" 
#  -quit
#kill %1

echo 'Attempting to zip builds'
zip -r $(pwd)/Build/windows.zip $(pwd)/Build/windows/
#zip -r $(pwd)/Build/mac.zip $(pwd)/Build/osx/
#zip -r $(pwd)/Build/linux.zip $(pwd)/Build/linux/
