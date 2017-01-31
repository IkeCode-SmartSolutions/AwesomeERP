﻿namespace Awe.Mvc.Core.Multitenancy
{
    public class TenantPipelineBuilderContext<TTenant>
    {
        public TenantContext<TTenant> TenantContext { get; set; }
        public TTenant Tenant { get; set; }
    }
}
