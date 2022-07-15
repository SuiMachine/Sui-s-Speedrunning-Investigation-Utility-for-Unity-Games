using System;
using System.ComponentModel;
using UnityEngine;

namespace SuisUnitySpeedrunningInvestigationUtility
{
	public class DisplayTrigger : MonoBehaviour
	{
		public static bool DisplayTriggers;
		static Mesh boxMesh;
		static Mesh sphereMesh;
		static Material TriggerMaterialEnabled;
		static Material TriggerMaterialDisabled;

		public static Transform RootNode;
		Collider triggerReference;
		TriggerType typeUsed;
		MeshRenderer meshRender;

		enum TriggerType
		{
			BoxCollider,
			SphereCollider,
			MeshCollider
		}

		internal static void GenerateTriggerMeshes()
		{
			DisplayTriggers = true;
			var boxColliders = Resources.FindObjectsOfTypeAll<BoxCollider>();
			foreach (var boxCol in boxColliders)
			{
				if (boxCol.gameObject.scene.buildIndex >= 0 && boxCol.isTrigger)
				{
					InitializeCollider(boxCol);
				}
			}

			var sphereColliders = Resources.FindObjectsOfTypeAll<SphereCollider>();
			foreach (var sphereCol in sphereColliders)
			{
				if (sphereCol.gameObject.scene.buildIndex >= 0 && sphereCol.isTrigger)
				{
					InitializeCollider(sphereCol);
				}
			}
		}

		private static void InitializeCollider(Collider collider)
		{
			if (boxMesh == null)
			{
				var tempObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
				var mf = tempObj.GetComponent<MeshFilter>();
				boxMesh = mf.sharedMesh;
				Destroy(tempObj);
				tempObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				mf = tempObj.GetComponent<MeshFilter>();
				sphereMesh = mf.sharedMesh;
				Destroy(tempObj);

				TriggerMaterialEnabled = new Material(Shader.Find("Unlit/Color"));
				TriggerMaterialEnabled.color = new Color(0, 0.2f, 0, 0.3f);
				TriggerMaterialDisabled = new Material(Shader.Find("Unlit/Color"));
				TriggerMaterialDisabled.color = new Color(0, 0.15f, 0.05f, 0.03f);
			}

			if (RootNode == null)
			{
				var go = new GameObject("Root Trigger DisplayNode");
				go.transform.position = Vector3.zero;
				go.transform.rotation = Quaternion.identity;
				go.transform.localScale = Vector3.one;
				RootNode = go.transform;
			}

			var displayTrigger = new GameObject(collider.gameObject.name).AddComponent<DisplayTrigger>();
			displayTrigger.triggerReference = collider;

			var addedMF = displayTrigger.gameObject.AddComponent<MeshFilter>();
			displayTrigger.meshRender = displayTrigger.gameObject.AddComponent<MeshRenderer>();
			displayTrigger.meshRender.sharedMaterial = TriggerMaterialEnabled;

			if (collider.GetType() == typeof(BoxCollider))
			{
				displayTrigger.typeUsed = TriggerType.BoxCollider;
				displayTrigger.transform.name = collider.gameObject.name + "-BoxTriggerDisplay";
				addedMF.sharedMesh = boxMesh;
			}
			else if (collider.GetType() == typeof(SphereCollider))
			{
				displayTrigger.typeUsed = TriggerType.SphereCollider;
				displayTrigger.transform.name = collider.gameObject.name + "-SphereTriggerDisplay";
				addedMF.sharedMesh = sphereMesh;
			}
			else if (collider.GetType() == typeof(MeshCollider))
			{
				displayTrigger.typeUsed = TriggerType.MeshCollider;
				displayTrigger.transform.name = collider.gameObject.name + "-MeshTriggerDisplay";

			}
			displayTrigger.transform.SetParent(RootNode);
		}

		public static void ToggleDisplay()
		{
			if (RootNode != null)
			{
				DisplayTriggers = !DisplayTriggers;
				RootNode.gameObject.SetActive(DisplayTriggers);
			}
		}

		void Update()
		{
			if (triggerReference == null)
				Destroy(this.gameObject);
			else
			{
				if (triggerReference.enabled && triggerReference.gameObject.activeInHierarchy)
					meshRender.material = TriggerMaterialEnabled;
				else
					meshRender.material = TriggerMaterialDisabled;

				switch (typeUsed)
				{
					case TriggerType.BoxCollider:
						{
							var boxCollider = (BoxCollider)triggerReference;
							this.transform.position = triggerReference.transform.position;
							this.transform.rotation = triggerReference.transform.rotation;
							this.transform.localScale = triggerReference.transform.lossyScale;

							this.transform.position += this.transform.rotation * boxCollider.center;
							//This is probably wrong, but I am too lazy to check the math
							this.transform.localScale = new Vector3(this.transform.localScale.x * boxCollider.size.x, this.transform.localScale.y * boxCollider.size.y, this.transform.localScale.z * boxCollider.size.z);
							break;
						}
					case TriggerType.SphereCollider:
						{
							var sphereCollider = (SphereCollider)triggerReference;
							this.transform.position = triggerReference.transform.position;
							this.transform.rotation = triggerReference.transform.rotation;
							this.transform.localScale = triggerReference.transform.lossyScale;

							this.transform.position += this.transform.rotation * sphereCollider.center;
							this.transform.localScale = new Vector3(this.transform.localScale.x * sphereCollider.radius, this.transform.localScale.y * sphereCollider.radius, this.transform.localScale.z * sphereCollider.radius);
							break;
						}
				}
			}
		}

	}
}
