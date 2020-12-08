//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.StreamAnalytics;
using Microsoft.Azure.StreamAnalytics.Serialization;
using StreamingContext = Microsoft.Azure.StreamAnalytics.StreamingContext;
using System.Text.Json;

namespace ASACustomDeserializerProject
{
    // Deserializes a stream into objects of type CustomEvent.
    // It reads the Stream line by line and assumes each line has three columns separated by ",".
    // Writes an error to diagnostics and skips the line otherwise.
    public class CustomJSONDeserializer : StreamDeserializer<SchoolData>
    {
        // streamingDiagnostics is used to write error to diagnostic logs
        private StreamingDiagnostics streamingDiagnostics;

        // Initializes the operator and provides context that is required for publishing diagnostics
        public override void Initialize(StreamingContext streamingContext)
        {
            this.streamingDiagnostics = streamingContext.Diagnostics;
        }

        // Deserializes a stream into objects of type CustomEvent
        public override IEnumerable<SchoolData> Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                var deserializeTask = JsonSerializer.DeserializeAsync<SchoolData>(stream);
                var schoolData = deserializeTask.Result; 
                yield return schoolData;
            }
        }
    }

    public class SchoolData
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string _school_id { get; set; }
        public Classroom[] classrooms { get; set; }
        public string points { get; set; }
        public string teacherName { get; set; }
        public string date { get; set; }
        public string averagepoints { get; set; }
        public int sumpoints { get; set; }
    }

    public class Classroom
    {
        public int id { get; set; }
    }   
}


