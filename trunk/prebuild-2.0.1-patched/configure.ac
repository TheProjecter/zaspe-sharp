
AC_INIT([Prebuild],[2.0.1])

AC_PREREQ(2.60)
AC_CANONICAL_SYSTEM
AC_CONFIG_AUX_DIR(.)
AM_INIT_AUTOMAKE([1.9 tar-ustar])
AM_MAINTAINER_MODE
dnl AC_PROG_INTLTOOL([0.25])
AC_PROG_INSTALL

ASSEMBLY_NAME=prebuild
PROJECT_NAME=Prebuild
PROJECT_VERSION=$VERSION
PROJECT_DESCRIPTION="The Prebuild project generator"
PROJECT_TYPE="Exe"

AC_SUBST(ASSEMBLY_NAME)
AC_SUBST(PROJECT_NAME)
AC_SUBST(PROJECT_VERSION)
AC_SUBST(DESCRIPTION)

AC_MSG_CHECKING([assembly type])
case $PROJECT_TYPE in
	*Exe)
		ASSEMBLY_EXTENSION=exe
		;;
	*Library)
		ASSEMBLY_EXTENSION=dll
		;;
	*)
		AC_MSG_ERROR([*** Please add support for project type $PROJECT_TYPE to configure.ac checks!])
		;;
esac
AC_MSG_RESULT([$PROJECT_TYPE])

AC_SUBST(ASSEMBLY_EXTENSION)

AC_MSG_CHECKING([whether we're compiling from an RCS])
if test -f "$srcdir/.cvs_version" ; then
        from_rcs=cvs
else
  if test -f "$srcdir/.svn/entries" ; then
  		from_rcs=svn
  else
        from_rcs=no
  fi
fi

AC_MSG_RESULT($from_rcs)

MONO_REQUIRED_VERSION=1.1

PKG_CHECK_MODULES(MONO_DEPENDENCY, mono >= $MONO_REQUIRED_VERSION, has_mono=true, has_mono=false)

if test "x$has_mono" = "xtrue"; then
  AC_PATH_PROG(RUNTIME, mono, no)
  AC_PATH_PROG(CSC, gmcs, no)
  if test `uname -s` = "Darwin"; then
        LIB_PREFIX=
        LIB_SUFFIX=.dylib
  else
        LIB_PREFIX=.so
        LIB_SUFFIX=
  fi
else
  AC_PATH_PROG(CSC, csc.exe, no)
  if test x$CSC = "xno"; then
        AC_MSG_ERROR([You need to install either mono or .Net])
  else
    RUNTIME=
    LIB_PREFIX=
    LIB_SUFFIX=
  fi
fi

AC_PATH_PROG(GACUTIL, gacutil)
if test "x$GACUTIL" = "xno" ; then
	AC_MSG_ERROR([No gacutil tool found])
fi

GACUTIL_FLAGS='/package prebuild /gacdir $(DESTDIR)$(prefix)/lib'
AC_SUBST(GACUTIL_FLAGS)

AC_SUBST(PATH)
AC_SUBST(LD_LIBRARY_PATH)

AC_SUBST(LIB_PREFIX)
AC_SUBST(LIB_SUFFIX)
AC_SUBST(RUNTIME)
AC_SUBST(CSC)

AC_SUBST(BASE_DEPENDENCIES_CFLAGS)
AC_SUBST(BASE_DEPENDENCIES_LIBS)

dnl Find monodoc
MONODOC_REQUIRED_VERSION=1.0
AC_SUBST(MONODOC_REQUIRED_VERSION)

PKG_CHECK_MODULES(MONODOC_DEPENDENCY, monodoc >= $MONODOC_REQUIRED_VERSION, enable_monodoc=yes, enable_monodoc=no)

if test "x$enable_monodoc" = "xyes"; then
        AC_PATH_PROG(MONODOC, monodoc, no)
        if test x$MONODOC = xno; then
           enable_monodoc=no
        fi
else
        MONODOC=
fi

AC_SUBST(MONODOC)
AM_CONDITIONAL(ENABLE_MONODOC, test "x$enable_monodoc" = "xyes")

winbuild=no
case "$host" in
       *-*-mingw*|*-*-cygwin*)
               winbuild=yes
               ;;
esac
AM_CONDITIONAL(WINBUILD, test x$winbuild = xyes)

AC_CONFIG_FILES(prebuild)


AC_CONFIG_FILES(Makefile)
AC_OUTPUT

echo "==="
echo ""
echo "Project configuration summary"
echo ""
echo "   * Installation prefix: $prefix"
echo "   * compiler:            $CSC"
echo "   * Documentation:       $enable_monodoc ($MONODOC)"
echo "   * Project Name:        $PROJECT_NAME"
echo "   * Version:             $PROJECT_VERSION"
echo ""
echo "==="


