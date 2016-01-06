using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MsgPack;
using MsgPack.Serialization;
using System;

public class TestMessagePack : MonoBehaviour
{


	public void OnGUI ()
	{
		if (GUI.Button (new Rect (0, 0, 100, 60), "Pack")) {
			var serializer = MessagePackSerializer.Get<A> ();
			byte[] res = serializer.PackSingleObject (new A {
				a = 1, b = 2, c = "testHelloWorld"
			});
			
			Debug.Log (res);
			Debug.Log (System.Convert.ToBase64String (res));
		}

		if (GUI.Button (new Rect (0, 70, 100, 60), "pack2")) {
			var s = MessagePackSerializer.Get<object[]> ();
			var list = new object[2];
			list [0] = new A {
			};
			list [1] = new B {
			};
			var res = s.PackSingleObject (list);
			Debug.Log (res);
			Debug.Log (System.Convert.ToBase64String (res));
		}

		if (GUI.Button (new Rect (0, 140, 100, 60), "pack hashtable")) {
			var s = MessagePackSerializer.Get<object[]> ();
			var arr = new object[2];
			arr [0] = new C {
				a = 1, b = 2, c = "CCC"
			};
			arr [1] = new D {
				a = 1, b = "DDD", c = "DDEEDD"
			};
			var res = s.PackSingleObject (arr);
			Debug.Log (res);
			Debug.Log (Convert.ToBase64String (res));
		}

		if (GUI.Button (new Rect (0, 210, 100, 60), "UnPack")) {
			var s = MessagePackSerializer.Get<object[][]> ();
			var res = System.Convert.FromBase64String ("kpMAAMCTAMDA");
			var data = s.UnpackSingleObject (res);
			Log (data);
		}

		if (GUI.Button (new Rect (0, 280, 100, 60), "UnPack2")) {
			var s = MessagePackSerializer.Get<Dictionary<string, object>[]> ();
			var res = Convert.FromBase64String ("koOhYQGhYgKhY6NDQ0ODoWEBoWKjREREoWOmRERFRURE");
			var data = s.UnpackSingleObject (res);
			Log2 (data);
		}
	}

	void Log (object d)
	{
		var data = d as object[][];
		foreach (var arr in data) {
			foreach (var val in arr) {
				Debug.Log (val);
			}
		}
	}

	void Log2 (object d)
	{
		var data = d as Dictionary<string, object>[];
		Debug.Log (data);
		foreach (var dic in data) {
			foreach (var key in dic) {
				Debug.Log (key);
			}
		}
	}

	public class A
	{
		public int a;
		public int b;
		public string c;
	}

	public class B
	{
		public int a;
		public string b;
		public string c;
	}


	public class C: IPackable, IUnpackable
	{
		public int a;
		public int b;
		public string c;

		public void PackToMessage (Packer packer, PackingOptions options)
		{
			// Pack fields are here:
			// First, record total fields size.
//			packer.PackArrayHeader (2);
//			packer.Pack (this.Id);
//			packer.PackString (this.Name);

			// ...Instead, you can pack as map as follows:
			packer.PackMapHeader (3);
			packer.Pack ("a");
			packer.Pack (this.a);
			packer.Pack ("b");
			packer.Pack (this.b);
			packer.Pack ("c");
			packer.Pack (this.c);
		}

		public void UnpackFromMessage (Unpacker unpacker)
		{
			// ...Instead, you can unpack from map as follows:
			if (!unpacker.IsMapHeader) {
				throw SerializationExceptions.NewIsNotMapHeader ();
			}

			// Check items count.
			if (UnpackHelpers.GetItemsCount (unpacker) != 3) {
				throw SerializationExceptions.NewUnexpectedArrayLength (3, UnpackHelpers.GetItemsCount (unpacker));
			}

			// Unpack fields here:
			for (int i = 0; i < 3 /* known count of fields */; i++) {
				// Unpack and verify key of entry in map.
				string key;
				if (!unpacker.ReadString (out key)) {
					// Missing key, incorrect.
					throw SerializationExceptions.NewUnexpectedEndOfStream ();
				}


				switch (key) {
				case "a":
					{
						int a;
						if (!unpacker.ReadInt32 (out a)) {
							throw SerializationExceptions.NewMissingProperty ("a");
						}
			
						this.a = a;
						break;
					}
				case "b":
					{
						int b;
						if (!unpacker.ReadInt32 (out b)) {
							throw SerializationExceptions.NewMissingProperty ("b");
						}
			
						this.b = b;
						break;
					}
				case "c":
					{
						string c;
						if (!unpacker.ReadString (out c)) {
							throw SerializationExceptions.NewMissingProperty ("c");
						}
			
						this.c = c;
						break;
					}

				// Note: You should ignore unknown fields for forward compatibility.
				}
			}
		}
	}

	public class D: IPackable, IUnpackable
	{
		public int a;
		public string b;
		public string c;

		public void PackToMessage (Packer packer, PackingOptions options)
		{
			// Pack fields are here:
			// First, record total fields size.
//			packer.PackArrayHeader (2);
//			packer.Pack (this.Id);
//			packer.PackString (this.Name);

			// ...Instead, you can pack as map as follows:
			packer.PackMapHeader (3);
			packer.Pack ("a");
			packer.Pack (this.a);
			packer.Pack ("b");
			packer.Pack (this.b);
			packer.Pack ("c");
			packer.Pack (this.c);
		}

		public void UnpackFromMessage (Unpacker unpacker)
		{
			// ...Instead, you can unpack from map as follows:
			if (!unpacker.IsMapHeader) {
				throw SerializationExceptions.NewIsNotMapHeader ();
			}

			// Check items count.
			if (UnpackHelpers.GetItemsCount (unpacker) != 3) {
				throw SerializationExceptions.NewUnexpectedArrayLength (3, UnpackHelpers.GetItemsCount (unpacker));
			}

			// Unpack fields here:
			for (int i = 0; i < 3 /* known count of fields */; i++) {
				// Unpack and verify key of entry in map.
				string key;
				if (!unpacker.ReadString (out key)) {
					// Missing key, incorrect.
					throw SerializationExceptions.NewUnexpectedEndOfStream ();
				}


				switch (key) {
				case "a":
					{
						int a;
						if (!unpacker.ReadInt32 (out a)) {
							throw SerializationExceptions.NewMissingProperty ("a");
						}
			
						this.a = a;
						break;
					}
				case "b":
					{
						string b;
						if (!unpacker.ReadString (out b)) {
							throw SerializationExceptions.NewMissingProperty ("b");
						}
			
						this.b = b;
						break;
					}
				case "c":
					{
						string c;
						if (!unpacker.ReadString (out c)) {
							throw SerializationExceptions.NewMissingProperty ("c");
						}
			
						this.c = c;
						break;
					}

				// Note: You should ignore unknown fields for forward compatibility.
				}
			}
		}
	}
}
