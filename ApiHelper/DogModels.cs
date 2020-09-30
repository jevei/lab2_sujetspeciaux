using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiHelper
{
    public class DogImagesModel
    {
        [JsonProperty("message")]
        public List<String> BreedPaths { get; set; }
    }

    public class DogImageModel
    {
        [JsonProperty("message")]
        public string BreedPath { get; set; }
    }

    /// <summary>
    /// Source : https://stackoverflow.com/questions/15789539/deserialize-array-of-key-value-pairs-using-json-net
    /// </summary>
    public class DogBreedsModel
    {
        [JsonProperty("message")]
        public Dictionary<string, List<string>> Breeds;
    }
}
