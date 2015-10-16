using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interop.ErpBS800;         // Use Primavera interop's [Path em C:\Program Files\Common Files\PRIMAVERA\SG800]
using Interop.StdPlatBS800;
using Interop.StdBE800;
using ADODB;
using Interop.IGcpBS800;

namespace FirstREST.Lib_Primavera
{
    public class PriEngine
    {

        public static StdPlatBS Platform { get; set; }
        public static ErpBS Engine { get; set; }

        public static bool InitializeCompany(string Company, string User, string Password)
        {
            Engine.
            StdBSConfApl objAplConf = new StdBSConfApl();
            StdPlatBS Plataforma = new StdPlatBS();
            ErpBS MotorLE = new ErpBS();

            EnumTipoPlataforma objTipoPlataforma = new EnumTipoPlataforma();
            objTipoPlataforma = EnumTipoPlataforma.tpProfissional;

            objAplConf.Instancia = "Default";
            objAplConf.AbvtApl = "GCP";
            objAplConf.PwdUtilizador = Password;
            objAplConf.Utilizador = User;

            StdBETransaccao objStdTransac = new StdBETransaccao();

            // Opem platform.
            Plataforma.AbrePlataformaEmpresaIntegrador(ref Company, ref objStdTransac, ref objAplConf, ref objTipoPlataforma);

            // Is plt initialized?
            if (Plataforma.Inicializada)
            {

                // Retuns the ptl.
                Platform = Plataforma;

                bool blnModoPrimario = true;

                // Open Engine
                MotorLE.AbreEmpresaTrabalho(EnumTipoPlataforma.tpProfissional, ref Company, ref User, ref Password, ref objStdTransac, "Default", ref blnModoPrimario);

                // Returns the engine.
                Engine = MotorLE;

                return true;
            }
            else
            {
                return false;
            }


        }

    }

}
