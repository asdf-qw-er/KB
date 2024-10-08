using System.IO;
using UnityEngine;

public class CreatePython : MonoBehaviour
{
    #region Life Cycle
    void Awake() { CopyPythonIfNotExists(); }
    #endregion

    #region Private
    #endregion

    #region Public
    public void CopyPythonIfNotExists()
    {
        string sourceFolder = Path.Combine(Application.streamingAssetsPath, "Python");
        string destinationFolder = Path.Combine(Application.persistentDataPath, "Python");

        if (!Directory.Exists(destinationFolder))
        {
            CopyDirectory(sourceFolder, destinationFolder);
            Debug.Log("Python copied to: " + destinationFolder);
        }
    }

    public void CopyDirectory(string sourceDir, string destinationDir)
    {
        Directory.CreateDirectory(destinationDir);

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            if (Path.GetExtension(file).ToLower() != ".meta")
            {
                string destFile = Path.Combine(destinationDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }
        }

        foreach (var dir in Directory.GetDirectories(sourceDir))
        {
            string destDir = Path.Combine(destinationDir, Path.GetFileName(dir));
            CopyDirectory(dir, destDir);
        }
    }
    #endregion
}
