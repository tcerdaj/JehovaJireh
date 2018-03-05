using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JehovaJireh.Web.UI.Models
{
    public class VerseViewModels
    {
        [JsonProperty(PropertyName = "auditid")]
        public string Auditid { get; set; }
        [JsonProperty(PropertyName = "verse")]
        public string Verse { get; set; }
        [JsonProperty(PropertyName = "lastverse")]
        public string Lastverse { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "osis_end")]
        public string Osis_end { get; set; }
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }
        [JsonProperty(PropertyName = "reference")]
        public string Reference { get; set; }
        [JsonProperty(PropertyName = "prev_osis_id")]
        public string Prev_osis_id { get; set; }
        [JsonProperty(PropertyName = "next_osis_id")]
        public string Next_osis_id { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "parent")]
        public ParentViewModel Parent { get; set; }
        [JsonProperty(PropertyName = "next")]
        public NextViewModel Next { get; set; }
        [JsonProperty(PropertyName = "previous")]
        public PreviousViewModel Previous { get; set; }
        [JsonProperty(PropertyName = "copyright")]
        public string Copyright { get; set; }
        [JsonProperty(PropertyName = "footnotes")]
        public List<FootNoteViewModel> Footnotes { get; set; }
        [JsonProperty(PropertyName = "crossreferences")]
        public List<CrossreferenceViewModel> Crossreferences { get; set; }
    }

    public class FootNoteViewModel
    {
       public Reference FootNote { get; set; }
    }

    public class CrossreferenceViewModel
    {
      public Reference Crossreference { get; set; }
    }

    public class Reference
    {
        [JsonProperty(PropertyName = "marker")]
        public string marker { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string content { get; set; }
        [JsonProperty(PropertyName = "note_id")]
        public string note_id { get; set; }
        [JsonProperty(PropertyName = "verse_id")]
        public string verse_id { get; set; }
    }

  
    public class ParentViewModel {
        public ChildViewModel Chapter { get; set; }
    }

    public class NextViewModel
    {
        public ChildViewModel Verse { get; set; }
    }

    public class PreviousViewModel
    {
        public ChildViewModel Verse { get; set; }
    }
    public class ChildViewModel
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }
}