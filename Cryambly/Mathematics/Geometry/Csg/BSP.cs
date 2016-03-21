﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Annotations;

namespace CryCil.Geometry.Csg
{
	/// <summary>
	/// Represents a node in a Binary Spatial Partitioning tree.
	/// </summary>
	/// <typeparam name="T">Type of elements in the tree.</typeparam>
	public class BspNode<T> where T : ISpatiallySplittable<T>
	{
		#region Properties
		/// <summary>
		/// Gets the plane that divides the 3D space at this node.
		/// </summary>
		/// <remarks>
		/// When this property is not initialized, its <see cref="CryCil.Geometry.Plane.D"/> is equal to
		/// <see cref="float.NaN"/> .
		/// </remarks>
		public Plane Plane { get; private set; }
		/// <summary>
		/// Gets the list of elements in this node.
		/// </summary>
		public List<T> Elements { get; private set; }
		/// <summary>
		/// Gets the node of the BSP tree that represents elements in front of this node.
		/// </summary>
		public BspNode<T> Front { get; private set; }
		/// <summary>
		/// Gets the node of the BSP tree that represents elements behind this node.
		/// </summary>
		public BspNode<T> Back { get; private set; }
		/// <summary>
		/// Returns a list of all elements in this BSP tree.
		/// </summary>
		public List<T> AllElements
		{
			get
			{
				List<T> allElements = new List<T>();
				allElements.AddRange(this.Elements);
				if (this.Front != null) allElements.AddRange(this.Front.AllElements);
				if (this.Back != null) allElements.AddRange(this.Back.AllElements);
				return allElements;
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates empty BSP tree.
		/// </summary>
		public BspNode()
		{
			this.Plane = new Plane(0, 0, 0, float.NaN);
			this.Elements = new List<T>();
		}
		/// <summary>
		/// Creates new BSP tree with this node as its root.
		/// </summary>
		/// <param name="elements">  A list of elements to add to the tree.</param>
		/// <param name="customData">
		/// An optional reference to data that can be used by a splitting algorithm.
		/// </param>
		public BspNode(IList<T> elements, [CanBeNull] object customData)
		{
			this.Plane = new Plane(0, 0, 0, float.NaN);
			this.Elements = new List<T>();

			this.AddElements(elements, customData);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Adds a list of elements to this tree.
		/// </summary>
		/// <param name="elements">  A list of elements to add.</param>
		/// <param name="customData">
		/// An optional reference to data that can be used by a splitting algorithm.
		/// </param>
		public void AddElements(IList<T> elements, [CanBeNull] object customData)
		{
			if (elements.IsNullOrEmpty())
			{
				return;
			}
			// Initialize a plane by picking first element's plane without too much heuristics.
			if (this.IsNotInitialized())
			{
				this.Plane = elements[0].GetPlane(customData);
			}
			// Split elements into appropriate groups.
			List<T> frontalElements = new List<T>();
			List<T> backElements = new List<T>();
			for (int i = 0; i < elements.Count; i++)
			{
				elements[i].Split(this.Plane,
								  this.Elements, // Coplanars are assigned to this node.
								  this.Elements, //
								  frontalElements, // This will go into front branch.
								  backElements, // This will go into back branch.
								  customData); // This might be a reference to the mesh object.
			}
			// Assign front and back branches.
			BspNode<T> t = this.Front;
			AssignBranch(ref t, frontalElements, customData);
			this.Front = t;
			t = this.Back;
			AssignBranch(ref t, backElements, customData);
			this.Back = t;
		}
		/// <summary>
		/// Inverts this BSP tree.
		/// </summary>
		public void Invert()
		{
			if (!this.IsNotInitialized())
			{
				this.Plane.Negate();
			}
			if (!this.Elements.IsNullOrEmpty())
			{
				this.Elements.ForEach(x => x.Invert());
			}

			this.Front?.Invert();
			this.Back?.Invert();

			BspNode<T> temp = this.Front;
			this.Front = this.Back;
			this.Back = temp;
		}
		/// <summary>
		/// Determines where given point is located relatively to this BSP tree.
		/// </summary>
		/// <param name="point"><see cref="Vector3"/> that represents a given point.</param>
		/// <returns>A position of the point.</returns>
		public BspPointPosition PointPosition(Vector3 point)
		{
			float distance = this.Plane.SignedDistance(point);
			// If given point is in front of this node's plane.
			if (distance > MathHelpers.ZeroTolerance)
			{
				// If there is nothing in front of the node, then the point is outside.
				return this.Front?.PointPosition(point) ?? BspPointPosition.Outside;
			}
			// If given point is behind this node's plane.
			if (distance < MathHelpers.NZeroTolerance)
			{
				// If there is nothing behind the node, then the point is inside.
				return this.Back?.PointPosition(point) ?? BspPointPosition.Inside;
			}
			// If given point is on a plane.

			// If this node has anything in front or behind it.
			if (this.Front != null || this.Back != null)
			{
				// If given point is outside of branching BSP trees, then it is outside of this one.
				if (this.Front != null && this.Front.PointPosition(point) != BspPointPosition.Outside ||
					this.Back != null && this.Back.PointPosition(point) != BspPointPosition.Outside)
				{
					// Otherwise it on the border of the tree.
					return BspPointPosition.Border;
				}
				return BspPointPosition.Outside;
			}
			// Otherwise it on the border of the tree.
			return BspPointPosition.Border;
		}
		/// <summary>
		/// Cuts given elements and removes parts that end up inside this tree.
		/// </summary>
		/// <remarks>
		/// Parts are "inside" this BSP tree when they are behind a node that doesn't have a back branch.
		/// </remarks>
		/// <param name="elements">  
		/// A list of elements to cut. This object is not modified in any way during execution.
		/// </param>
		/// <param name="customData">
		/// An optional reference to data that can be used by a splitting algorithm.
		/// </param>
		/// <returns>A list that contains elements that are outside of this tree.</returns>
		public List<T> FilterList(IList<T> elements, [CanBeNull] object customData)
		{
			if (this.IsNotInitialized())
			{
				return new List<T>(elements);
			}
			// Prepare the lists.
			List<T> fronts = new List<T>();
			List<T> backs = new List<T>();
			// Cut elements and separate them into 2 lists.
			for (int i = 0; i < elements.Count; i++)
			{
				elements[i].Split(this.Plane, fronts, backs, fronts, backs, customData);
			}

			if (this.Front != null)
			{
				fronts = this.Front.FilterList(fronts, customData);
			}
			// If this node has nothing behind it in the tree, then whatever is behind it in the list should
			// be discarded.
			if (this.Back != null)
			{
				fronts.AddRange(this.Back.FilterList(backs, customData));
			}
			return fronts;
		}
		/// <summary>
		/// Cuts elements inside this tree and removes ones that end up inside another one.
		/// </summary>
		/// <param name="bsp">       A root of another BSP tree.</param>
		/// <param name="customData">
		/// An optional reference to data that can be used by a splitting algorithm.
		/// </param>
		public void CutTreeOut(BspNode<T> bsp, [CanBeNull] object customData)
		{
			this.Elements = bsp.FilterList(this.Elements, customData);
			this.Front?.CutTreeOut(bsp, customData);
			this.Back?.CutTreeOut(bsp, customData);
		}
		/// <summary>
		/// Adds given BSP tree to this one.
		/// </summary>
		/// <param name="another">   BSP tree to add.</param>
		/// <param name="customData">
		/// An optional reference to data that can be used by a splitting algorithm.
		/// </param>
		public void Unite(BspNode<T> another, [CanBeNull] object customData)
		{
			// Cut overlapping geometry.
			this.CutTreeOut(another, customData);
			another.CutTreeOut(this, customData);
			// Remove overlapping coplanar faces.
			another.Invert();
			another.CutTreeOut(this, customData);
			another.Invert();
			// Combine geometry.
			this.AddElements(another.AllElements, customData);
		}
		#endregion
		#region Utilities
		private static void AssignBranch(ref BspNode<T> branch, IList<T> elements, [CanBeNull] object customData)
		{
			if (branch == null)
			{
				// This is not a redundant assignment, since we are dealing with a reference type.
				branch = new BspNode<T>(elements, customData);
			}
			else
			{
				branch.AddElements(elements, customData);
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsNotInitialized()
		{
			return float.IsNaN(this.Plane.D);
		}
		#endregion
	}
	/// <summary>
	/// Defines common functionality of objects that are located in 3D space and can be split up.
	/// </summary>
	/// <typeparam name="T">Type of elements this object can split into.</typeparam>
	public interface ISpatiallySplittable<T>
	{
		/// <summary>
		/// Gets a plane this object can be considered to be located on.
		/// </summary>
		/// <param name="customData">Optional data that can be used in calculations.</param>
		/// <returns>
		/// <see cref="Plane"/> object that describes the plane this object is located on.
		/// </returns>
		Plane GetPlane([CanBeNull] object customData = null);
		/// <summary>
		/// Gets a normal to a plane this object can be considered to be located on.
		/// </summary>
		/// <param name="customData">Optional data that can be used in calculations.</param>
		/// <returns>Normalized <see cref="Vector3"/> that represents a normal.</returns>
		Vector3 GetNormal([CanBeNull] object customData = null);
		/// <summary>
		/// When implemented in derived type, splits this object into parts.
		/// </summary>
		/// <param name="splitter">             
		/// <see cref="Plane"/> that splits the space that contains this object.
		/// </param>
		/// <param name="frontCoplanarElements">
		/// An optional collection for parts of this object that are located on this plane and face the same
		/// way.
		/// </param>
		/// <param name="backCoplanarElements"> 
		/// An optional collection for parts of this object that are located on this plane and face the
		/// opposite way.
		/// </param>
		/// <param name="frontElements">        
		/// An optional collection for parts of this object that are located in front of this plane.
		/// </param>
		/// <param name="backElements">         
		/// An optional collection for parts of this object that are located behind this plane.
		/// </param>
		/// <param name="customData">           
		/// Optional object that provides data elements can be bound to.
		/// </param>
		void Split(Plane splitter,
				   ICollection<T> frontCoplanarElements,
				   ICollection<T> backCoplanarElements,
				   ICollection<T> frontElements,
				   ICollection<T> backElements,
				   [CanBeNull] object customData = null);
		/// <summary>
		/// When implemented, inverts orientation of this object.
		/// </summary>
		void Invert();
	}
	/// <summary>
	/// Enumeration of positions a point can have in relation to a BSP tree.
	/// </summary>
	public enum BspPointPosition
	{
		/// <summary>
		/// The point is outside of the BSP tree.
		/// </summary>
		Outside,
		/// <summary>
		/// The point is inside the BSP tree.
		/// </summary>
		Inside,
		/// <summary>
		/// The point is on the border of the BSP tree.
		/// </summary>
		Border
	}
}