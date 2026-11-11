namespace CapaVista_Componentes.Componentes
{
    partial class ClsButtonCompras
    {
        private System.ComponentModel.IContainer _Componentes = null;

        protected override void Dispose(bool LiberarRecursos)
        {
            if (LiberarRecursos) _FuenteModo?.Dispose();
            if (LiberarRecursos && (_Componentes != null))
            {
                _Componentes.Dispose();
            }
            base.Dispose(LiberarRecursos);
        }

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            _Componentes = new System.ComponentModel.Container();
        }

        #endregion
    }
}
