SOLUTION_NAME = VRCSaveTextManager
SOLUTION_FILE = $(SOLUTION_NAME).sln
MAIN_PROJECT_FILE = $(SOLUTION_NAME)\$(SOLUTION_NAME).csproj
PROJECT_DIRS = $(SOLUTION_NAME) Koturn.VRChat.Log\Koturn.VRChat.Log Koturn.Windows.Consoles\Koturn.Windows.Consoles
ARTIFACTS_BASEDIR = Artifacts
ARTIFACTS_SUBDIR_BASENAME = $(SOLUTION_NAME)
ARTIFACTS_BASENAME = $(SOLUTION_NAME)
BUILD_CONFIG = Release
TARGET_NET9 = net9.0-windows
TARGET_NFW481 = net481
SINGLE_SUFFIX = -single
RM = del /F /Q
RMDIR = rmdir /S /Q


all: build

build:
	dotnet build -c $(BUILD_CONFIG) $(SOLUTION_FILE)

restore:
	dotnet restore $(SOLUTION_FILE)

deploy: deploy-$(TARGET_NET9)

deploy$(SINGLE_SUFFIX): deploy-$(TARGET_NET9)$(SINGLE_SUFFIX)

deploy-$(TARGET_NET9):
	-dotnet publish -c $(BUILD_CONFIG) -f $(TARGET_NET9) -r win-x64 \
		-p:TargetFramework=$(TARGET_NET9) \
		-p:PublishDir=..\$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9) \
		-p:PublishAot=false \
		$(MAIN_PROJECT_FILE)
	-$(RM) $(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9)\*.pdb \
		$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9)\*.xml \
		$(ARTIFACTS_BASENAME)-$(TARGET_NET9).zip 2>NUL
	cd $(ARTIFACTS_BASEDIR)
	powershell Compress-Archive -Path $(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9) -DestinationPath ..\$(ARTIFACTS_BASENAME)-$(TARGET_NET9).zip
	cd $(MAKEDIR)

deploy-$(TARGET_NET9)$(SINGLE_SUFFIX):
	-dotnet publish -c $(BUILD_CONFIG) -f $(TARGET_NET9) -r win-x64 \
		-p:TargetFramework=$(TARGET_NET9) \
		-p:PublishDir=..\$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9)$(SINGLE_SUFFIX) \
		-p:PublishAot=false \
		-p:PublishSingleFile=true \
		-p:PublishReadyToRun=true \
		--self-contained=true \
		$(MAIN_PROJECT_FILE)
	-$(RM) $(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9)$(SINGLE_SUFFIX)\*.pdb \
		$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9)$(SINGLE_SUFFIX)\*.xml \
		$(ARTIFACTS_BASENAME)-$(TARGET_NET9)$(SINGLE_SUFFIX).zip 2>NUL
	cd $(ARTIFACTS_BASEDIR)
	powershell Compress-Archive -Path $(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NET9)$(SINGLE_SUFFIX) -DestinationPath ..\$(ARTIFACTS_BASENAME)-$(TARGET_NET9)$(SINGLE_SUFFIX).zip
	cd $(MAKEDIR)

deploy-$(TARGET_NFW481):
	-dotnet publish -c $(BUILD_CONFIG) -f $(TARGET_NFW481) \
		-p:TargetFramework=$(TARGET_NFW481) \
		-p:PublishDir=..\$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NFW481) \
		-p:PublishAot=false \
		$(MAIN_PROJECT_FILE)
	-$(RM) $(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NFW481)\*.pdb \
		$(ARTIFACTS_BASEDIR)\$(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NFW481)\*.xml \
		$(ARTIFACTS_BASENAME)-$(TARGET_NFW481).zip 2>NUL
	cd $(ARTIFACTS_BASEDIR)
	powershell Compress-Archive -Path $(ARTIFACTS_SUBDIR_BASENAME)-$(TARGET_NFW481) -DestinationPath ..\$(ARTIFACTS_BASENAME)-$(TARGET_NFW481).zip
	cd $(MAKEDIR)

clean:
	-for %%d in ( $(PROJECT_DIRS) ) do @( \
		@$(RMDIR) %%d\bin %%d\obj 2>NUL \
	)

distclean: clean
	-$(RMDIR) $(ARTIFACTS_BASEDIR) 2>NUL
	-$(RM) $(ARTIFACTS_BASENAME)-*.zip 2>NUL
