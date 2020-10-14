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
                case RuntimePlatform.WebGLPlayer:
                    return Application.streamingAssetsPath;
                case RuntimePlatform.Android:
					return "jar:file://" + Application.dataPath + "!/assets/";
				case RuntimePlatform.OSXPlayer:
					return "file://" + Application.dataPath + "/Data/StreamingAssets/";
				default:
					return Application.streamingAssetsPath;
            }
		}

        [DllImport("__Internal")]
        internal static extern string LoadFile(string url);

    }
}

#endif