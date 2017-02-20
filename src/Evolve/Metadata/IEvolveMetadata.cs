﻿using Evolve.Migration;
using System.Collections.Generic;
using System;

namespace Evolve.Metadata
{
    public interface IEvolveMetadata
    {
        /// <summary>
        ///     Lock the access to the metadata store to others migration processes.
        ///     Only one migration at a time is authorized.
        /// </summary>
        void Lock();

        /// <summary>
        ///     Create the metadata store if not exists.
        /// </summary>
        /// <returns> True if created, false if it already exists. </returns>
        bool CreateIfNotExists();

        /// <summary>
        ///     Save the metadata of an executed migration.
        /// </summary>
        /// <param name="migration"> The migration script metadata. </param>
        /// <param name="success"> True if the migration succeeded, false otherwise. </param>
        void SaveMigration(MigrationScript migration, bool success);

        /// <summary>
        ///     <para>
        ///         Save generic Evolve metadata.
        ///     </para>
        ///     <para>
        ///         Use <see cref="MetadataType.NewSchema"/> when Evolve has created the schema.
        ///     </para>
        ///     <para>
        ///         Use <see cref="MetadataType.EmptySchema"/> when the schema already exists but is empty when Evolve first run.
        ///     </para>
        ///     <para>
        ///         Use <see cref="MetadataType.StartVersion"/> to define a version used as a starting point for the future migration.
        ///     </para>
        /// </summary>
        /// <param name="type"> Metadata type to save. Cannot be null. </param>
        /// <param name="version"> Version of the record . Cannot be null. </param>
        /// <param name="description"> Metadata description. Cannot be null. </param>
        /// <param name="name"> Metadata name. Cannot be null. </param>
        /// <exception cref="ArgumentException">
        ///     Throws ArgumentException when the type of the metadata to save is <see cref="MetadataType.Migration"/>. 
        /// </exception>
        void Save(MetadataType type, string version, string description, string name);

        /// <summary>
        ///     Returns all the migration metadata.
        /// </summary>
        /// <returns> The list of all migration metadata. </returns>
        IEnumerable<MigrationMetadata> GetAllMigrationMetadata();

        /// <summary>
        ///     <para>
        ///         Returns True if Evolve can drop the schema, false otherwise.
        ///     </para>
        ///     <para>
        ///         Evolve can drop the schema if it created it in the first place.
        ///     </para>
        /// </summary>
        /// <returns> True if Evolve can drop the schema, false otherwise. </returns>
        bool CanDropSchema(string schemaName);

        /// <summary>
        ///     <para>
        ///         Returns True if Evolve can clean the schema, false otherwise.
        ///     </para>
        ///     <para>
        ///         Evolve can clean the schema if it was empty when it first run.
        ///     </para>
        /// </summary>
        /// <returns> True if Evolve can clean the schema, false otherwise. </returns>
        bool CanCleanSchema(string schemaName);

        /// <summary>
        ///     <para>
        ///         Returns the version where the migration shall begin. (default: 0)
        ///     </para>
        ///     <para>
        ///         All the migration scripts prior to this mark are ignored.
        ///     </para>
        /// </summary>
        /// <returns> The migration starting point. </returns>
        MigrationVersion FindStartVersion();
    }
}
