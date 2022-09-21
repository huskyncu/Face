using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacePractice
{
    class PersonObj
    {
        private string _personId;
        private string _name;
        private string _gender;
        private string _age;
        private string _confidence;
        private string[] _faceIds;

        public PersonObj(string personId, string name,string[] faceIds)
        {
            _personId = personId;
            _name = name;
            _faceIds = faceIds;
        }

        public PersonObj(string personId,  string gender, string age, string confidence)
        {
            _personId = personId;
            _gender = gender;
            _age = age;
            _confidence = confidence;
        }

        public PersonObj(string personId, string name, string gender, string age, string confidence)
        {
            _personId = personId;
            _name = name;
            _gender = gender;
            _age = age;
            _confidence = confidence;
        }

        public string PersonId { get => _personId; set => _personId = value; }
        public string Name { get => _name; set => _name = value; }
        public string Gender { get => _gender; set => _gender = value; }
        public string Age { get => _age; set => _age = value; }
        public string Confidence { get => _confidence; set => _confidence = value; }
        public string[] FaceIds { get => _faceIds; set => _faceIds = value; }
        
        public void SetNameAndFaceIds(string combineStr)
        {
            string[] tmp = combineStr.Split(',');
            Name = tmp[0];
            FaceIds = new string[tmp.Length-1];
            for(int i = 0; i < FaceIds.Length; i++)
            {
                if(!tmp[i + 1].Equals(""))
                {
                    FaceIds[i] = tmp[i + 1];
                }
            }
        }
        public static string formatConfidence(string confidence)
        {
            return String.Format("{0:N2}", Math.Round(Double.Parse(confidence) * 100, MidpointRounding.AwayFromZero).ToString() + "%");
        }
    }
}
