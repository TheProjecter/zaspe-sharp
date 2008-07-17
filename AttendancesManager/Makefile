
EXTRA_DIST =  Makefile

# Warning: This is an automatically generated file, do not edit!

srcdir=.
top_srcdir=..

include $(top_srcdir)/config.make

ifeq ($(CONFIG),RELEASE)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ -debug -define:DEBUG
ASSEMBLY = bin/Release/AttendancesManager.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../EventsManager/bin/Release/EventsManager.dll \
	../PersonsManager/bin/Release/PersonsManager.dll \
	../EventTypesManager/bin/Release/EventTypesManager.dll
BUILD_DIR = bin/Release/

GENTLE_PROVIDER_SQLITE_DLL_SOURCE=../Gentle.NET/Gentle.Provider.SQLite.dll
LOG4NET_DLL_SOURCE=../Gentle.NET/log4net.dll
GENTLE_COMMON_DLL_SOURCE=../Gentle.NET/Gentle.Common.dll
EVENTTYPESMANAGER_DLL_MDB_SOURCE=../EventTypesManager/bin/Release/EventTypesManager.dll.mdb
GENTLE_FRAMEWORK_DLL_SOURCE=../Gentle.NET/Gentle.Framework.dll
EVENTTYPESMANAGER_DLL_SOURCE=../EventTypesManager/bin/Release/EventTypesManager.dll

endif

ifeq ($(CONFIG),DEBUG)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:TRACE;DEBUG"

ASSEMBLY = bin/Debug/AttendancesManager.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../EventsManager/bin/Debug/EventsManager.dll \
	../PersonsManager/bin/Debug/PersonsManager.dll \
	../EventTypesManager/bin/Debug/EventTypesManager.dll
BUILD_DIR = bin/Debug/

GENTLE_PROVIDER_SQLITE_DLL_SOURCE=../Gentle.NET/Gentle.Provider.SQLite.dll
LOG4NET_DLL_SOURCE=../Gentle.NET/log4net.dll
GENTLE_COMMON_DLL_SOURCE=../Gentle.NET/Gentle.Common.dll
EVENTTYPESMANAGER_DLL_MDB_SOURCE=../EventTypesManager/bin/Debug/EventTypesManager.dll.mdb
GENTLE_FRAMEWORK_DLL_SOURCE=../Gentle.NET/Gentle.Framework.dll
EVENTTYPESMANAGER_DLL_SOURCE=../EventTypesManager/bin/Debug/EventTypesManager.dll

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=.resources.dll

PROGRAMFILES = \
	$(GENTLE_PROVIDER_SQLITE_DLL) \
	$(LOG4NET_DLL) \
	$(GENTLE_COMMON_DLL) \
	$(EVENTTYPESMANAGER_DLL_MDB) \
	$(GENTLE_FRAMEWORK_DLL) \
	$(EVENTTYPESMANAGER_DLL)  

LINUX_PKGCONFIG = \
	$(ATTENDANCESMANAGER_PC)  



GENTLE_PROVIDER_SQLITE_DLL = $(BUILD_DIR)/Gentle.Provider.SQLite.dll
LOG4NET_DLL = $(BUILD_DIR)/log4net.dll
GENTLE_COMMON_DLL = $(BUILD_DIR)/Gentle.Common.dll
ATTENDANCESMANAGER_PC = $(BUILD_DIR)/attendancesmanager.pc
EVENTTYPESMANAGER_DLL_MDB = $(BUILD_DIR)/EventTypesManager.dll.mdb
GENTLE_FRAMEWORK_DLL = $(BUILD_DIR)/Gentle.Framework.dll
EVENTTYPESMANAGER_DLL = $(BUILD_DIR)/EventTypesManager.dll

FILES = \
	AssemblyInfo.cs \
	Attendance.Properties.cs \
	Attendance.cs \
	AttendanceExceptions.cs \
	AttendanceManager.Methods.cs \
	AttendanceManager.Properties.cs \
	AttendanceManager.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	attendancesmanager.pc.in 

REFERENCES =  \
	System

DLL_REFERENCES =  \
	../Gentle.NET/Gentle.Common.dll \
	../Gentle.NET/Gentle.Framework.dll \
	../Gentle.NET/Gentle.Provider.SQLite.dll \
	../Gentle.NET/log4net.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

#Targets
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG)  $(top_srcdir)/config.make

include $(top_srcdir)/Makefile.include
#include $(srcdir)/custom-hooks.make



$(eval $(call emit-deploy-target,GENTLE_PROVIDER_SQLITE_DLL))
$(eval $(call emit-deploy-target,LOG4NET_DLL))
$(eval $(call emit-deploy-target,GENTLE_COMMON_DLL))
$(eval $(call emit-deploy-wrapper,ATTENDANCESMANAGER_PC,attendancesmanager.pc))
$(eval $(call emit-deploy-target,EVENTTYPESMANAGER_DLL_MDB))
$(eval $(call emit-deploy-target,GENTLE_FRAMEWORK_DLL))
$(eval $(call emit-deploy-target,EVENTTYPESMANAGER_DLL))


$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(build_resx_resources) : %.resources: %.resx
	resgen2 '$<' '$@'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	make pre-all-local-hook prefix=$(prefix)
	mkdir -p $(dir $(ASSEMBLY))
	make $(CONFIG)_BeforeBuild
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
	make $(CONFIG)_AfterBuild
	make post-all-local-hook prefix=$(prefix)

install-local: $(ASSEMBLY) $(ASSEMBLY_MDB) $(GENTLE_PROVIDER_SQLITE_DLL) $(LOG4NET_DLL) $(GENTLE_COMMON_DLL) $(ATTENDANCESMANAGER_PC) $(EVENTTYPESMANAGER_DLL_MDB) $(GENTLE_FRAMEWORK_DLL) $(EVENTTYPESMANAGER_DLL)
	make pre-install-local-hook prefix=$(prefix)
	mkdir -p $(DESTDIR)$(libdir)/$(PACKAGE)
	cp $(ASSEMBLY) $(ASSEMBLY_MDB) $(DESTDIR)$(libdir)/$(PACKAGE)
	test -z '$(GENTLE_PROVIDER_SQLITE_DLL)' || cp $(GENTLE_PROVIDER_SQLITE_DLL) $(DESTDIR)$(libdir)/$(PACKAGE)
	test -z '$(LOG4NET_DLL)' || cp $(LOG4NET_DLL) $(DESTDIR)$(libdir)/$(PACKAGE)
	test -z '$(GENTLE_COMMON_DLL)' || cp $(GENTLE_COMMON_DLL) $(DESTDIR)$(libdir)/$(PACKAGE)
	mkdir -p $(DESTDIR)$(libdir)/pkgconfig
	test -z '$(ATTENDANCESMANAGER_PC)' || cp $(ATTENDANCESMANAGER_PC) $(DESTDIR)$(libdir)/pkgconfig
	test -z '$(EVENTTYPESMANAGER_DLL_MDB)' || cp $(EVENTTYPESMANAGER_DLL_MDB) $(DESTDIR)$(libdir)/$(PACKAGE)
	test -z '$(GENTLE_FRAMEWORK_DLL)' || cp $(GENTLE_FRAMEWORK_DLL) $(DESTDIR)$(libdir)/$(PACKAGE)
	test -z '$(EVENTTYPESMANAGER_DLL)' || cp $(EVENTTYPESMANAGER_DLL) $(DESTDIR)$(libdir)/$(PACKAGE)
	make post-install-local-hook prefix=$(prefix)

uninstall-local: $(ASSEMBLY) $(ASSEMBLY_MDB) $(GENTLE_PROVIDER_SQLITE_DLL) $(LOG4NET_DLL) $(GENTLE_COMMON_DLL) $(ATTENDANCESMANAGER_PC) $(EVENTTYPESMANAGER_DLL_MDB) $(GENTLE_FRAMEWORK_DLL) $(EVENTTYPESMANAGER_DLL)
	make pre-uninstall-local-hook prefix=$(prefix)
	rm -f $(DESTDIR)$(libdir)/$(PACKAGE)/$(notdir $(ASSEMBLY))
	test -z '$(ASSEMBLY_MDB)' || rm -f $(DESTDIR)$(libdir)/$(PACKAGE)/$(notdir $(ASSEMBLY_MDB))
	test -z '$(GENTLE_PROVIDER_SQLITE_DLL)' || rm -f $(DESTDIR)$(libdir)/$(PACKAGE)/$(notdir $(GENTLE_PROVIDER_SQLITE_DLL))
	test -z '$(LOG4NET_DLL)' || rm -f $(DESTDIR)$(libdir)/$(PACKAGE)/$(notdir $(LOG4NET_DLL))
	test -z '$(GENTLE_COMMON_DLL)' || rm -f $(DESTDIR)$(libdir)/$(PACKAGE)/$(notdir $(GENTLE_COMMON_DLL))
	test -z '$(ATTENDANCESMANAGER_PC)' || rm -f $(DESTDIR)$(libdir)/pkgconfig/$(notdir $(ATTENDANCESMANAGER_PC))
	test -z '$(EVENTTYPESMANAGER_DLL_MDB)' || rm -f $(DESTDIR)$(libdir)/$(PACKAGE)/$(notdir $(EVENTTYPESMANAGER_DLL_MDB))
	test -z '$(GENTLE_FRAMEWORK_DLL)' || rm -f $(DESTDIR)$(libdir)/$(PACKAGE)/$(notdir $(GENTLE_FRAMEWORK_DLL))
	test -z '$(EVENTTYPESMANAGER_DLL)' || rm -f $(DESTDIR)$(libdir)/$(PACKAGE)/$(notdir $(EVENTTYPESMANAGER_DLL))
	make post-uninstall-local-hook prefix=$(prefix)

install: install-local
uninstall: uninstall-local
clean: clean-local

include $(top_srcdir)/rules.make
