using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using HospitalSystem_WebAPI_dotnet6.ViewModels;
using HospitalSystem_WebAPI_dotnet6.Entity;

namespace HospitalSystem_WebAPI_dotnet6.Utils {

    public class JWTUtils {

        private static string secret => "Zd+kZozTI5OgURtbegh8E6KTPghNNe/tEFwuLxd2UNw=";
         
        public static string CreateToken(Dictionary<string, object> payload) {
            JwtEncoder encoder = new JwtEncoder(new HMACSHA256Algorithm(), new JsonNetSerializer(), new JwtBase64UrlEncoder());
            return encoder.Encode(payload, secret);
        }

        public static T GetPatientFromToken<T>(string token) where T : class {

            IJsonSerializer serializer = new JsonNetSerializer();
            IJwtValidator validator = new JwtValidator(serializer, new UtcDateTimeProvider());
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();

            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

            var payload = new Dictionary<string, object>(decoder.DecodeToObject(token, secret, verify: true));


            if (typeof(T).Equals(typeof(PatientView))) {
                PatientView patientView = new PatientView {
                    ID = (string)payload["id"],
                    Name = (string)payload["name"],
                    Sex = (string)payload["sex"],
                    Age = Convert.ToInt32(payload["age"]),
                    Birthdate = (DateTime)payload["birthdate"]
                };                
                return (T)(object)patientView;
            }
            else if (typeof(T).Equals(typeof(Patient))) {
                Patient patient = new Patient {
                    ID = (string)payload["id"],
                    Name = (string)payload["name"],
                    Sex = (string)payload["sex"],
                    Birthdate = (DateTime)payload["birthdate"]
                };
                return (T)(object)patient;
            }
            else {
                return null;
            }
        }
    }
}
