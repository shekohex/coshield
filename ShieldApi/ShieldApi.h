// ShieldApi.h

#pragma once

using namespace System;
using namespace System::Reflection;

namespace ShieldApi {

	public ref class ShieldApiC
	{
        public:void RunMyShield()
        {            
            Shield::Main myShield;
			myShield.Run();
            return;
        }
    };
}
__declspec(dllexport) void RunShieldCpp()
{
    ShieldApi::ShieldApiC work;    
    work.RunMyShield();    
}