using BaseWinform.AccesAction;
using Cataloger.Business.ParametresSystemes;
using Cataloger.Core.Entities.SystemsParameters.Dto;

namespace Cataloger.Presenters
{
    public class SystemParameterService : PresenterDirectAccessAction
    {
        public SystemParameterService() {}

        // Création de tous les champs dans la BD lors de la réception complète
        public bool CreateSystemParameter(SystemParameterDto systemParameter, bool displayMsg = false)
        {
            bool success = false;

            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
            post<SystemParameterDto>("systems-parameters", systemParameter, out _,
                Succes: (dto) =>
                {
                    success = true;
                    return displayMsg;
                }
            );
            return success;
        }

        public bool ModifySystemParameter(SystemParameterDto systemParameter, bool displayMsg = false)
        {
            bool success = false;

            put<SystemParameterDto>($"systems-parameters/{systemParameter.SystemParameterId}", systemParameter, out _,
                Succes: (dto) =>
                {
                    success = true;
                    return displayMsg;
                }    
            );
            return success;
        }

        public IEnumerable<SystemParameterDto> GetListSystemParameter()
        {
            IEnumerable<SystemParameterDto>? dtos = null;

            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
            get("systems-parameters", out dtos);
            return dtos!;
        }

        public IEnumerable<SystemParameterDto> GetListParametrSystemParameterFromSection(string section)
        {
            IEnumerable<SystemParameterDto>? dtos = null;

            get($"systems-parameters/sections/{section}", out dtos);
            return dtos!;
        }

        public SystemParameterDto? GetSystemParameter(string section, string key)
        {
            SystemParameterDto? dto = null;

            get($"systems-parameters/sections/{section}/keys/{key}", out dto);
            return dto;
        }

        public bool DeleteSystemParameter(string section, string? key = null, bool displayMsg = false)
        {
            bool succes = false;

            if (key != null)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                delete(
                    $"systems-parameters/sections/{section}/keys/{key}",
                    Succes: () => { succes = true; return displayMsg; }
                );
            }
            else
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                delete(
                    $"systems-parameters/sections/{section}",
                    Succes: () => { succes = true; return displayMsg; }
                );
            }

            return succes;
        }
    }
}
