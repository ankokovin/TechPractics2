using System.Collections.Generic;
using System.Data;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public abstract class Repos<T>
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
        public abstract DataTable table(IEnumerable<T> enumerable);
    }
}