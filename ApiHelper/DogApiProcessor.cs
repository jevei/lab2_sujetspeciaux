using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiHelper
{
    public class DogApiProcessor
    {

        public static async Task<List<string>> LoadBreedList()
        {
            ///TODO : À compléter LoadBreedList
            /// Attention le type de retour n'est pas nécessairement bon
            /// J'ai mis quelque chose pour avoir une base
            /// TODO : Compléter le modèle manquant

            //return new List<string>();
            string url = "https://dog.ceo/api/breeds/list/all";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    DogBreedsModel result = await response.Content.ReadAsAsync<DogBreedsModel>();
                    var races = new List<string>();
                    foreach(var breed in result.Breeds)
                    {
                        if (breed.Value.Count > 0)
                        {
                            foreach(var underBreed in breed.Value)
                            { 
                                races.Add(breed.Key + "/" + underBreed); 
                            }
                        }
                        else
                        {
                            races.Add(breed.Key);
                        }
                    }
                    return races;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<List<string>> LoadDogImage(string nb, string breed)
        {
            int numberOfImages = Convert.ToInt32(nb);
            string url = await GetImageUrl(breed);

            if (numberOfImages > 1)
                url += $"/{numberOfImages}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    if (numberOfImages > 1)
                    {
                        DogImagesModel result = await response.Content.ReadAsAsync<DogImagesModel>();
                        return result.BreedPaths;
                    }
                    else
                    {
                        DogImageModel result = await response.Content.ReadAsAsync<DogImageModel>();

                        return new List<string> { result.BreedPath };
                    }
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<string> GetImageUrl(string breed)
        {
            /// TODO : GetImageUrl()
            /// TODO : Compléter le modèle manquant
            //return string.Empty;
            string url = "https://dog.ceo/api/breed/"+breed+"/images/random";
            return url;
        }
    }
}
