#if UNITY

using System.Runtime.InteropServices;
using UnityEngine;

namespace NLog.Internal
{
	internal class UnityHelper
	{
		internal static string GetStreamingAssetPath()
		{
			switch (Application.platform)
			{
				case RuntimePlatform.IPhonePlayer:
					return "file://" + Application.dataPath + "/Raw/";
				case RuntimePlatform.Android:
					return "jar:file://" + Application.dataPath + "!/assets/";
				case RuntimePlatform.OSXPlayer:
					return "file://" + Application.dataPath + "/Data/StreamingAssets/";
                case RuntimePlatform.WebGLPlayer:
                    return Application.streamingAssetsPath;
				default:
					return "file://" + Application.dataPath + "/StreamingAssets/";
			}
		}

        [DllImport("__Internal")]
        internal static extern string LoadFile(string url);

    }
}

#endif