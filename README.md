Digitize your surroundings with Oculus controller using Passtrought mode.

------------------------------------------------------------------------------------


Description

	Created in inspiration of https://github.com/fabio914/PassthroughMeasure.

	Place a points in the air by the right controller and save their positions to the .obj file. 
	
	Press Right/Left controller's trigger for placing points.
	Right controller joystick: Forward - switch for save/poining, Right - delete the last point.
	
	After that it's possible to access the file using SideQuest app, where the path on the Oculus should be: 
		/sdcard/Android/data/com.CosmoPlan.DotSpace/files/points.obj
	
	Using Blender editor it's possible to import that file using import settings like these: 
	/ Transform / 
	Forward: Y Forward, 
	Up: Z Up (because of different types of axis system of Unity 3d and Blender). 
	
	The output line format is like this: 
		writer.WriteLine("v {0} {1} {2}", point.transform.position.x,
		 point.transform.position.z, point.transform.position.y); 
	So this sequence: x, z, y - should be good for Blender's import.


------------------------------------------------------------------------------------

Useful links:

Passtrought Setup:
	https://neerajjaiswal.com/how-to-setup-passthrough-api-for-quest-in-unity


OpenXR + OVR Passtrought:
	https://forum.unity.com/threads/oculus-mixed-reality-passthrough-mode-api-via-xr-integration.1163992/

Project building/compilation error (sometimes happens and depends on Unity version and host OS)
https://forums.oculusvr.com/t5/Oculus-Go-Development/NullReferenceException-from-Oculus-package/td-p/758571/page/2

	" I fix this problem:
	
	filnd below lines  in OVRPlugin.cs:
	
	private const string pluginName = "OVRPlugin";
	private static System.Version _versionZero = new System.Version(0, 0, 0);
	
	and move them to line 43 (my version is 18.0.0).
	
	43: private const string pluginName = "OVRPlugin";
	44: private static System.Version _versionZero = new System.Version(0, 0, 0);
	45: 
	46: #if OVRPLUGIN_UNSUPPORTED_PLATFORM
	47:     public static readonly System.Version wrapperVersion = _versionZero;
	48: #else
	49:     public static readonly System.Version wrapperVersion = OVRP_1_49_0.version;
	50: #endif "
	
