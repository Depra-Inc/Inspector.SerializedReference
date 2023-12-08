﻿// SPDX-License-Identifier: Apache-2.0
// © 2023 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Reflection;
using Depra.Inspector.SerializedReference.Editor.Exceptions;
using UnityEditor;

namespace Depra.Inspector.SerializedReference.Editor.Extensions
{
	internal static class SerializePropertyExtensions
	{
		public static Type GetManagedReferenceFieldType(this SerializedProperty property) =>
			property.propertyType != SerializedPropertyType.ManagedReference
				? throw new SerializedPropertyTypeMustBeManagedReference(nameof(property))
				: GetType(property.managedReferenceFieldTypename);

		public static Type GetManagedReferenceType(this SerializedProperty property) =>
			property.propertyType != SerializedPropertyType.ManagedReference
				? throw new SerializedPropertyTypeMustBeManagedReference(nameof(property))
				: GetType(property.managedReferenceFullTypename);

		public static object SetManagedReference(this SerializedProperty property, Type type)
		{
			var @object = type != null ? Activator.CreateInstance(type) : null;
			property.managedReferenceValue = @object;

			return @object;
		}

		private static Type GetType(string typeName)
		{
			var splitIndex = typeName.IndexOf(' ');
			var assembly = Assembly.Load(typeName[..splitIndex]);

			return assembly.GetType(typeName[(splitIndex + 1)..]);
		}
	}
}