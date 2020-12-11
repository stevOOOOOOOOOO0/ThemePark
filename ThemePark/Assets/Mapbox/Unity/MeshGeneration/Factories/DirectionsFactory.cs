<<<<<<< HEAD
using System;

=======
>>>>>>> 70a0f9f16ad177f4b21438af99ef17e541da6143
namespace Mapbox.Unity.MeshGeneration.Factories
{
	using UnityEngine;
	using Mapbox.Directions;
	using System.Collections.Generic;
	using System.Linq;
	using Mapbox.Unity.Map;
	using Data;
	using Modifiers;
	using Mapbox.Utils;
	using Mapbox.Unity.Utilities;
	using System.Collections;

	public class DirectionsFactory : MonoBehaviour
	{
<<<<<<< HEAD

		public FloatData duration, arrivalMinute, arrivalHour, distance;

=======
>>>>>>> 70a0f9f16ad177f4b21438af99ef17e541da6143
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		MeshModifier[] MeshModifiers;
		[SerializeField]
		Material _material;

		[SerializeField]
<<<<<<< HEAD
		public Transform[] _waypoints;
=======
		Transform[] _waypoints;
>>>>>>> 70a0f9f16ad177f4b21438af99ef17e541da6143
		private List<Vector3> _cachedWaypoints;

		[SerializeField]
		[Range(1,10)]
		private float UpdateFrequency = 2;



		private Directions _directions;
		private int _counter;

		GameObject _directionsGO;
		private bool _recalculateNext;

		protected virtual void Awake()
		{
			if (_map == null)
			{
				_map = FindObjectOfType<AbstractMap>();
			}
			_directions = MapboxAccess.Instance.Directions;
			_map.OnInitialized += Query;
			_map.OnUpdated += Query;
		}

		public void Start()
		{
			_cachedWaypoints = new List<Vector3>(_waypoints.Length);
			foreach (var item in _waypoints)
			{
				_cachedWaypoints.Add(item.position);
			}
			_recalculateNext = false;

			foreach (var modifier in MeshModifiers)
			{
				modifier.Initialize();
			}

			StartCoroutine(QueryTimer());
		}

		protected virtual void OnDestroy()
		{
			_map.OnInitialized -= Query;
			_map.OnUpdated -= Query;
		}

		void Query()
		{
			var count = _waypoints.Length;
			var wp = new Vector2d[count];
			for (int i = 0; i < count; i++)
			{
				wp[i] = _waypoints[i].GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
			}
<<<<<<< HEAD
			var _directionResource = new DirectionResource(wp, RoutingProfile.Walking);
			_directionResource.Steps = true;
			_directions.Query(_directionResource, HandleDirectionsResponse);
			var route = new Route();

=======
			var _directionResource = new DirectionResource(wp, RoutingProfile.Driving);
			_directionResource.Steps = true;
			_directions.Query(_directionResource, HandleDirectionsResponse);
>>>>>>> 70a0f9f16ad177f4b21438af99ef17e541da6143
		}

		public IEnumerator QueryTimer()
		{
			while (true)
			{
				yield return new WaitForSeconds(UpdateFrequency);
				for (int i = 0; i < _waypoints.Length; i++)
				{
					if (_waypoints[i].position != _cachedWaypoints[i])
					{
						_recalculateNext = true;
						_cachedWaypoints[i] = _waypoints[i].position;
					}
				}

				if (_recalculateNext)
				{
					Query();
					_recalculateNext = false;
				}
			}
		}

		void HandleDirectionsResponse(DirectionsResponse response)
		{
			if (response == null || null == response.Routes || response.Routes.Count < 1)
			{
				return;
			}

			var meshData = new MeshData();
			var dat = new List<Vector3>();
			foreach (var point in response.Routes[0].Geometry)
			{
				dat.Add(Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
			}

			var feat = new VectorFeatureUnity();
			feat.Points.Add(dat);

			foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
			{
				mod.Run(feat, meshData, _map.WorldRelativeScale);
			}

			CreateGameObject(meshData);
<<<<<<< HEAD
			
			//set the duration of the travel;
			duration.data = 0f;
			foreach (var i in response.Routes)
			{
				duration.data += (float)i.Duration;
			}
			
			duration.data /= 60;
			duration.data -= duration.data % 1;
			duration.data++;
			
			//set the Arrival time
			arrivalMinute.data = (DateTime.Now.Minute + duration.data) % 60f;

			float tempTime = (DateTime.Now.Minute + duration.data) / 60f;
			
			if (tempTime > 1)
			{
				tempTime -= tempTime % 1;
				arrivalHour.data = DateTime.Now.Hour + tempTime;
			}
			else
			{
				arrivalHour.data = DateTime.Now.Hour;
			}
			
			
			//set the distance to travel
			foreach (var j in response.Routes)
			{
				distance.data += (float)j.Distance;
			}

			distance.data = distance.data / 1609;
			distance.data = (float)Math.Round(distance.data, 2);

=======
>>>>>>> 70a0f9f16ad177f4b21438af99ef17e541da6143
		}

		GameObject CreateGameObject(MeshData data)
		{
			if (_directionsGO != null)
			{
				_directionsGO.Destroy();
			}
			_directionsGO = new GameObject("direction waypoint " + " entity");
			var mesh = _directionsGO.AddComponent<MeshFilter>().mesh;
			mesh.subMeshCount = data.Triangles.Count;

			mesh.SetVertices(data.Vertices);
			_counter = data.Triangles.Count;
			for (int i = 0; i < _counter; i++)
			{
				var triangle = data.Triangles[i];
				mesh.SetTriangles(triangle, i);
			}

			_counter = data.UV.Count;
			for (int i = 0; i < _counter; i++)
			{
				var uv = data.UV[i];
				mesh.SetUVs(i, uv);
			}

			mesh.RecalculateNormals();
			_directionsGO.AddComponent<MeshRenderer>().material = _material;
			return _directionsGO;
		}
	}

}
