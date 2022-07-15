using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuisUnitySpeedrunningInvestigationUtility
{
	public class HackGuiThing : MonoBehaviour
	{
		public static HackGuiThing Instance;
		private string process;

		public static void Initialize()
		{
			if(Instance == null)
			{
				SuisUnitySpeedrunningInvestigationUtility.loggerInst.Error("Initialize");
				var go = new GameObject("Yes");
				DontDestroyOnLoad(go);
				Instance = go.AddComponent<HackGuiThing>();
			}
		}

		public void OnGUI()
		{
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical(GUI.skin.box);
			//GUILayout.Label($"Current process: {process}");
			GUILayout.Label($"Display Triggers: {DisplayTrigger.DisplayTriggers}");
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F7))
			{
				if (Input.GetKey(KeyCode.LeftShift))
					DisplayTrigger.GenerateTriggerMeshes();
				else
				{
					if(DisplayTrigger.RootNode == null)
						DisplayTrigger.GenerateTriggerMeshes();
					else
						DisplayTrigger.ToggleDisplay();
				}
			}

			/*			if(Input.GetKeyDown(KeyCode.F7) && Input.GetKey(KeyCode.LeftShift))
						{
							process = "Writting down";
							StartCoroutine(StoreShit());
						}*/
		}

		private System.Collections.IEnumerator StoreShit()
		{
			yield return null;
			var meshesRenderer = FindObjectsOfType<MeshRenderer>();
			SuisUnitySpeedrunningInvestigationUtility.loggerInst.Msg($"Found {meshesRenderer.Length} mesh renderers");
			var boxColliders = FindObjectsOfType<BoxCollider>();
			SuisUnitySpeedrunningInvestigationUtility.loggerInst.Msg($"Found {boxColliders.Length} box colliders");
			var sphereColliders = FindObjectsOfType<SphereCollider>();
			SuisUnitySpeedrunningInvestigationUtility.loggerInst.Msg($"Found {sphereColliders.Length} sphere colliders");
			var meshColliders = FindObjectsOfType<MeshCollider>();
			SuisUnitySpeedrunningInvestigationUtility.loggerInst.Msg($"Found {meshColliders.Length} mesh colliders");
			var terrain = FindObjectsOfType<Terrain>();
			SuisUnitySpeedrunningInvestigationUtility.loggerInst.Msg($"Found {meshColliders.Length} mesh colliders");

			EssentialData data = new EssentialData()
			{
				dataStandardMesh = new List<StandardMesh>(),
				dataCollisionBox = new List<CollisionDataBox>(),
				dataCollisionMesh = new List<CollisionDataMesh>(),
				dataCollisionSphere = new List<CollisionDataSphere>()
			};

/*			for (int i = 0; i < meshesRenderer.Length; i++)
			{
				var mr = meshesRenderer[i];
				var mf = mr.GetComponent<MeshFilter>();
				if (mf == null || mf.sharedMesh == null)
					continue;

				data.dataStandardMesh.Add(new StandardMesh(mr.name, mr.transform.position, mr.transform.rotation, mr.transform.lossyScale, mf.sharedMesh.vertices, mf.sharedMesh.triangles));
			}
			process = "Mesh ready";
			yield return null;

			for (int i = 0; i < boxColliders.Length; i++)
			{
				var boxCollider = boxColliders[i];
				data.dataCollisionBox.Add(new CollisionDataBox(boxCollider.name, boxCollider.transform.position, boxCollider.transform.rotation, boxCollider.transform.lossyScale, boxCollider.center, boxCollider.size));
			}
			process = "Box ready";
			yield return null;

			for (int i = 0; i < sphereColliders.Length; i++)
			{
				var sphere = sphereColliders[i];
				data.dataCollisionSphere.Add(new CollisionDataSphere(sphere.name, sphere.transform.position, sphere.transform.rotation, sphere.transform.lossyScale, sphere.center, sphere.radius));
			}
			process = "Sphere ready";
			yield return null;

			for (int i = 0; i < meshColliders.Length; i++)
			{
				var mc = meshColliders[i];
				if (mc.sharedMesh == null)
					continue;
				data.dataCollisionMesh.Add(new CollisionDataMesh(mc.name, mc.transform.position, mc.transform.rotation, mc.transform.lossyScale, mc.sharedMesh.vertices, mc.sharedMesh.triangles));
			}
			process = "Collision mesh ready";
			yield return null;*/

			var writter = new StreamWriter(SceneManager.GetActiveScene().name + ".xml");
			XmlSerializer serializer = new XmlSerializer(typeof(EssentialData));
			serializer.Serialize(writter, data);
			writter.Close();
			process = "Done";
		}

		[Serializable]
		public struct EssentialData
		{
			public List<StandardMesh> dataStandardMesh;
			public List<CollisionDataBox> dataCollisionBox;
			public List<CollisionDataSphere> dataCollisionSphere;
			public List<CollisionDataMesh> dataCollisionMesh;
			public List<CollisionDataMesh> dataCollisionTerrain;
		}

		[Serializable]
		public struct StandardMesh
		{
			public string Name;
			public Vector3 Position;
			public Quaternion Rotation;
			public Vector3 GlobalScale;
			public Vector3[] Verts;
			public int[] Triangles;

			public StandardMesh(string Name, Vector3 Position, Quaternion Rotation, Vector3 GlobalScale, Vector3[] Verts, int[] Triangles)
			{
				this.Name = Name;
				this.Position = Position;
				this.Rotation = Rotation;
				this.GlobalScale = GlobalScale;
				this.Verts = Verts;
				this.Triangles = Triangles;
			}
		}

		[Serializable]
		public struct CollisionDataBox
		{
			public string Name;
			public Vector3 Position;
			public Quaternion Rotation;
			public Vector3 GlobalScale;
			public Vector3 Center;
			public Vector3 Size;

			public CollisionDataBox(string Name, Vector3 Position, Quaternion Rotation, Vector3 GlobalScale, Vector3 Center, Vector3 Size)
			{
				this.Name = Name;
				this.Position = Position;
				this.Rotation = Rotation;
				this.GlobalScale = GlobalScale;
				this.Center = Center;
				this.Size = Size;
			}
		}

		[Serializable]
		public struct CollisionDataSphere
		{
			public string Name;
			public Vector3 Position;
			public Quaternion Rotation;
			public Vector3 GlobalScale;
			public Vector3 Center;
			public float Radius;

			public CollisionDataSphere(string Name, Vector3 Position, Quaternion Rotation, Vector3 GlobalScale, Vector3 Center, float Radius)
			{
				this.Name = Name;
				this.Position = Position;
				this.Rotation = Rotation;
				this.GlobalScale = GlobalScale;
				this.Center = Center;
				this.Radius = Radius;
			}
		}

		[Serializable]
		public struct CollisionDataMesh
		{
			public string Name;
			public Vector3 Position;
			public Quaternion Rotation;
			public Vector3 GlobalScale;
			public Vector3[] Verts;
			public int[] Triangles;


			public CollisionDataMesh(string Name, Vector3 Position, Quaternion Rotation, Vector3 GlobalScale, Vector3[] Verts, int[] Triangles)
			{
				this.Name = Name;
				this.Position = Position;
				this.Rotation = Rotation;
				this.GlobalScale = GlobalScale;
				this.Verts = Verts;
				this.Triangles = Triangles;
			}
		}

		[Serializable]
		public struct CollisionDataTerrain
		{
			public string Name;
			public Vector3 Position;
			public Quaternion Rotation;
			public Vector3 GlobalScale;
			public Vector3[] Verts;
			public int[] Triangles;


			public CollisionDataTerrain(string Name, Vector3 Position, Quaternion Rotation, Vector3 GlobalScale, Vector3[] Verts, int[] Triangles)
			{
				this.Name = Name;
				this.Position = Position;
				this.Rotation = Rotation;
				this.GlobalScale = GlobalScale;
				this.Verts = Verts;
				this.Triangles = Triangles;
			}
		}
	}
}