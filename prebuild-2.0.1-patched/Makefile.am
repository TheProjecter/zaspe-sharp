


ASSEMBLY=$(ASSEMBLY_NAME).$(ASSEMBLY_EXTENSION)


prebuilddir = $(prefix)/lib/prebuild
prebuild_DATA = $(ASSEMBLY)

bin_SCRIPTS=prebuild


PACKAGES =
BINARY_LIBS =
SYSTEM_LIBS = \
	System.EnterpriseServices \
	System.Xml \
	System
RESOURCES = \
	App.ico \
	data/prebuild-1.7.xsd \
	data/autotools.xml
SOURCES = \
	Prebuild.cs \
	Core/FatalException.cs \
	Core/Kernel.cs \
	Core/UnknownLanguageException.cs \
	Core/WarningException.cs \
	Core/Attributes/DataNodeAttribute.cs \
	Core/Attributes/OptionNodeAttribute.cs \
	Core/Attributes/TargetAttribute.cs \
	Core/Interfaces/IDataNode.cs \
	Core/Interfaces/ITarget.cs \
	Core/Nodes/AuthorNode.cs \
	Core/Nodes/ConfigurationNode.cs \
	Core/Nodes/DataNode.cs \
	Core/Nodes/DescriptionNode.cs \
	Core/Nodes/ExcludeNode.cs \
	Core/Nodes/FileNode.cs \
	Core/Nodes/FilesNode.cs \
	Core/Nodes/MatchNode.cs \
	Core/Nodes/OptionsNode.cs \
	Core/Nodes/ProcessNode.cs \
	Core/Nodes/ProjectNode.cs \
	Core/Nodes/ReferenceNode.cs \
	Core/Nodes/ReferencePathNode.cs \
	Core/Nodes/SolutionNode.cs \
	Core/Parse/IfContext.cs \
	Core/Parse/Preprocessor.cs \
	Core/Targets/AutotoolsTarget.cs \
	Core/Targets/DebugTarget.cs \
	Core/Targets/MonoDevelopTarget.cs \
	Core/Targets/NAntTarget.cs \
	Core/Targets/SharpDevelop2Target.cs \
	Core/Targets/SharpDevelopTarget.cs \
	Core/Targets/VS2002Target.cs \
	Core/Targets/VS2003Target.cs \
	Core/Targets/VS2005Target.cs \
	Core/Utilities/CommandLineCollection.cs \
	Core/Utilities/CurrentDirectory.cs \
	Core/Utilities/Helper.cs \
	Core/Utilities/Log.cs \
	Properties/AssemblyInfo.cs

EXTRA_DIST=$(SOURCES) $(BINARY_LIBS) install-sh missing  \
	App.ico \
	data/prebuild-1.7.xsd \
	data/autotools.xml

CLEANFILES=$(ASSEMBLY)






bin/Debug/$(ASSEMBLY): $(RESOURCES) $(SOURCES) $(BINARY_LIBS) Prebuild.snk
	mkdir -p bin/Debug && $(CSC) /out:$@ \
		/target:exe \
		$(addprefix /resource:$(srcdir)/, $(RESOURCES)) \
		$(addprefix /r:, $(SYSTEM_LIBS)) \
		$(addprefix $(srcdir)/, $(SOURCES)) \
		/keyfile:$(srcdir)/Prebuild.snk \
	&& rm -f $(ASSEMBLY) \
	&& ln $@ $(ASSEMBLY)
	
CLEANFILES+=bin/Debug/$(ASSEMBLY)


EXTRA_DIST+=Prebuild.snk

Debug: bin/Debug/$(ASSEMBLY)
	rm -f $(ASSEMBLY) \
	&& ln bin/Debug/$(ASSEMBLY) $(ASSEMBLY)





bin/Release/$(ASSEMBLY): $(RESOURCES) $(SOURCES) $(BINARY_LIBS) Prebuild.snk
	mkdir -p bin/Release && $(CSC) /out:$@ \
		/target:exe \
		$(addprefix /resource:$(srcdir)/, $(RESOURCES)) \
		$(addprefix /r:, $(SYSTEM_LIBS)) \
		$(addprefix $(srcdir)/, $(SOURCES)) \
		/keyfile:$(srcdir)/Prebuild.snk \
	&& rm -f $(ASSEMBLY) \
	&& ln $@ $(ASSEMBLY)
	
CLEANFILES+=bin/Release/$(ASSEMBLY)


EXTRA_DIST+=Prebuild.snk

Release: bin/Release/$(ASSEMBLY)
	rm -f $(ASSEMBLY) \
	&& ln bin/Release/$(ASSEMBLY) $(ASSEMBLY)





$(ASSEMBLY): Debug

all: $(ASSEMBLY)



