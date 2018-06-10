using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class Repos
    {
        protected Model1Container cont;
        protected bool AllowCascade;
        protected bool CheckInputs;
        public Repos(Model1Container model, bool checkInputs = true, bool allowCascade = false)
        {
            cont = model;
            CheckInputs = checkInputs;
            AllowCascade = allowCascade;
        }
    }
}