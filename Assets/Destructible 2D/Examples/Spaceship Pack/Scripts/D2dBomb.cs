using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dBomb))]
	public class D2dBomb_Editor : D2dEditor<D2dBomb>
	{
		protected override void OnInspector()
		{
			DrawDefault("_Bomb");
            DrawDefault("Bus");
            DrawDefault("X");
            DrawDefault("Y");
        }
	}
}
#endif

namespace Destructible2D
{
	public class D2dBomb : MonoBehaviour
	{
		[Tooltip("The BOMB prefab spawned when shooting")]
		public GameObject _Bomb;

        [Tooltip("The BUSffff")]
        public GameObject Bus;

        [Tooltip("PlusX")]
        public float X;

        public float Y;

        public int number;

        public void Shoot()
		{
           
            var prefab = Instantiate(_Bomb, new Vector3(Bus.transform.position.x + X, Bus.transform.position.y + 0.04f + Y, 0), transform.rotation);

            prefab.name = "BOMB" + number++;
            prefab.GetComponent<Rigidbody2D>().velocity = Bus.GetComponent<Rigidbody2D>().velocity;
            prefab.GetComponent<Rigidbody2D>().mass = 0.3f;
            //prefab.GetComponent<CircleCollider2D>().enabled = false;

        }       
	}
}