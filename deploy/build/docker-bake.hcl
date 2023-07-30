target "default" {
	context    = "../.."
	dockerfile = "./deploy/build/dockerfile"
	platforms  = [
		"linux/amd64",
#		"linux/arm64"
	]
	tags = [
		"elyspio/mantis-devops-bridge:latest"
	]
	args = {
		SLN_PATH         = "back/MantisDevopsBridge.Api.sln"
		MAIN_CSPROJ_PATH = "Web/MantisDevopsBridge.Api.Web.csproj"
		ROOT_FOLDER      = "back/"
		ENTRY_DLL        = "MantisDevopsBridge.Api.Web.dll"
	}
}