using System;

namespace Khronos;

#pragma warning disable IDE1006 // Naming Styles

internal static unsafe class OpenGL
{
    public delegate nint FunctionLoader(string name);

    public static void glInitialize(FunctionLoader functionLoader)
    {
        static nint LoadFunction(FunctionLoader functionLoader, string name)
        {
            nint value = functionLoader(name);
            if (value == 0)
            {
                throw new Exception($"Could not load required opengl function '{name}'");
            }
            return value;
        }
        pfn_glActiveTexture = LoadFunction(functionLoader, "glActiveTexture");
        pfn_glAttachShader = LoadFunction(functionLoader, "glAttachShader");
        pfn_glBindAttribLocation = LoadFunction(functionLoader, "glBindAttribLocation");
        pfn_glBindBuffer = LoadFunction(functionLoader, "glBindBuffer");
        pfn_glBindFramebuffer = LoadFunction(functionLoader, "glBindFramebuffer");
        pfn_glBindRenderbuffer = LoadFunction(functionLoader, "glBindRenderbuffer");
        pfn_glBindTexture = LoadFunction(functionLoader, "glBindTexture");
        pfn_glBlendColor = LoadFunction(functionLoader, "glBlendColor");
        pfn_glBlendEquation = LoadFunction(functionLoader, "glBlendEquation");
        pfn_glBlendEquationSeparate = LoadFunction(functionLoader, "glBlendEquationSeparate");
        pfn_glBlendFunc = LoadFunction(functionLoader, "glBlendFunc");
        pfn_glBlendFuncSeparate = LoadFunction(functionLoader, "glBlendFuncSeparate");
        pfn_glBufferData = LoadFunction(functionLoader, "glBufferData");
        pfn_glBufferSubData = LoadFunction(functionLoader, "glBufferSubData");
        pfn_glCheckFramebufferStatus = LoadFunction(functionLoader, "glCheckFramebufferStatus");
        pfn_glClear = LoadFunction(functionLoader, "glClear");
        pfn_glClearColor = LoadFunction(functionLoader, "glClearColor");
        pfn_glClearDepthf = LoadFunction(functionLoader, "glClearDepthf");
        pfn_glClearStencil = LoadFunction(functionLoader, "glClearStencil");
        pfn_glColorMask = LoadFunction(functionLoader, "glColorMask");
        pfn_glCompileShader = LoadFunction(functionLoader, "glCompileShader");
        pfn_glCompressedTexImage2D = LoadFunction(functionLoader, "glCompressedTexImage2D");
        pfn_glCompressedTexSubImage2D = LoadFunction(functionLoader, "glCompressedTexSubImage2D");
        pfn_glCopyTexImage2D = LoadFunction(functionLoader, "glCopyTexImage2D");
        pfn_glCopyTexSubImage2D = LoadFunction(functionLoader, "glCopyTexSubImage2D");
        pfn_glCreateProgram = LoadFunction(functionLoader, "glCreateProgram");
        pfn_glCreateShader = LoadFunction(functionLoader, "glCreateShader");
        pfn_glCullFace = LoadFunction(functionLoader, "glCullFace");
        pfn_glDeleteBuffers = LoadFunction(functionLoader, "glDeleteBuffers");
        pfn_glDeleteFramebuffers = LoadFunction(functionLoader, "glDeleteFramebuffers");
        pfn_glDeleteProgram = LoadFunction(functionLoader, "glDeleteProgram");
        pfn_glDeleteRenderbuffers = LoadFunction(functionLoader, "glDeleteRenderbuffers");
        pfn_glDeleteShader = LoadFunction(functionLoader, "glDeleteShader");
        pfn_glDeleteTextures = LoadFunction(functionLoader, "glDeleteTextures");
        pfn_glDepthFunc = LoadFunction(functionLoader, "glDepthFunc");
        pfn_glDepthMask = LoadFunction(functionLoader, "glDepthMask");
        pfn_glDepthRangef = LoadFunction(functionLoader, "glDepthRangef");
        pfn_glDetachShader = LoadFunction(functionLoader, "glDetachShader");
        pfn_glDisable = LoadFunction(functionLoader, "glDisable");
        pfn_glDisableVertexAttribArray = LoadFunction(functionLoader, "glDisableVertexAttribArray");
        pfn_glDrawArrays = LoadFunction(functionLoader, "glDrawArrays");
        pfn_glDrawElements = LoadFunction(functionLoader, "glDrawElements");
        pfn_glEnable = LoadFunction(functionLoader, "glEnable");
        pfn_glEnableVertexAttribArray = LoadFunction(functionLoader, "glEnableVertexAttribArray");
        pfn_glFinish = LoadFunction(functionLoader, "glFinish");
        pfn_glFlush = LoadFunction(functionLoader, "glFlush");
        pfn_glFramebufferRenderbuffer = LoadFunction(functionLoader, "glFramebufferRenderbuffer");
        pfn_glFramebufferTexture2D = LoadFunction(functionLoader, "glFramebufferTexture2D");
        pfn_glFrontFace = LoadFunction(functionLoader, "glFrontFace");
        pfn_glGenBuffers = LoadFunction(functionLoader, "glGenBuffers");
        pfn_glGenerateMipmap = LoadFunction(functionLoader, "glGenerateMipmap");
        pfn_glGenFramebuffers = LoadFunction(functionLoader, "glGenFramebuffers");
        pfn_glGenRenderbuffers = LoadFunction(functionLoader, "glGenRenderbuffers");
        pfn_glGenTextures = LoadFunction(functionLoader, "glGenTextures");
        pfn_glGetActiveAttrib = LoadFunction(functionLoader, "glGetActiveAttrib");
        pfn_glGetActiveUniform = LoadFunction(functionLoader, "glGetActiveUniform");
        pfn_glGetAttachedShaders = LoadFunction(functionLoader, "glGetAttachedShaders");
        pfn_glGetAttribLocation = LoadFunction(functionLoader, "glGetAttribLocation");
        pfn_glGetBooleanv = LoadFunction(functionLoader, "glGetBooleanv");
        pfn_glGetBufferParameteriv = LoadFunction(functionLoader, "glGetBufferParameteriv");
        pfn_glGetError = LoadFunction(functionLoader, "glGetError");
        pfn_glGetFloatv = LoadFunction(functionLoader, "glGetFloatv");
        pfn_glGetFramebufferAttachmentParameteriv = LoadFunction(functionLoader, "glGetFramebufferAttachmentParameteriv");
        pfn_glGetIntegerv = LoadFunction(functionLoader, "glGetIntegerv");
        pfn_glGetProgramiv = LoadFunction(functionLoader, "glGetProgramiv");
        pfn_glGetProgramInfoLog = LoadFunction(functionLoader, "glGetProgramInfoLog");
        pfn_glGetRenderbufferParameteriv = LoadFunction(functionLoader, "glGetRenderbufferParameteriv");
        pfn_glGetShaderiv = LoadFunction(functionLoader, "glGetShaderiv");
        pfn_glGetShaderInfoLog = LoadFunction(functionLoader, "glGetShaderInfoLog");
        pfn_glGetShaderPrecisionFormat = LoadFunction(functionLoader, "glGetShaderPrecisionFormat");
        pfn_glGetShaderSource = LoadFunction(functionLoader, "glGetShaderSource");
        pfn_glGetString = LoadFunction(functionLoader, "glGetString");
        pfn_glGetTexParameterfv = LoadFunction(functionLoader, "glGetTexParameterfv");
        pfn_glGetTexParameteriv = LoadFunction(functionLoader, "glGetTexParameteriv");
        pfn_glGetUniformfv = LoadFunction(functionLoader, "glGetUniformfv");
        pfn_glGetUniformiv = LoadFunction(functionLoader, "glGetUniformiv");
        pfn_glGetUniformLocation = LoadFunction(functionLoader, "glGetUniformLocation");
        pfn_glGetVertexAttribfv = LoadFunction(functionLoader, "glGetVertexAttribfv");
        pfn_glGetVertexAttribiv = LoadFunction(functionLoader, "glGetVertexAttribiv");
        pfn_glGetVertexAttribPointerv = LoadFunction(functionLoader, "glGetVertexAttribPointerv");
        pfn_glHint = LoadFunction(functionLoader, "glHint");
        pfn_glIsBuffer = LoadFunction(functionLoader, "glIsBuffer");
        pfn_glIsEnabled = LoadFunction(functionLoader, "glIsEnabled");
        pfn_glIsFramebuffer = LoadFunction(functionLoader, "glIsFramebuffer");
        pfn_glIsProgram = LoadFunction(functionLoader, "glIsProgram");
        pfn_glIsRenderbuffer = LoadFunction(functionLoader, "glIsRenderbuffer");
        pfn_glIsShader = LoadFunction(functionLoader, "glIsShader");
        pfn_glIsTexture = LoadFunction(functionLoader, "glIsTexture");
        pfn_glLineWidth = LoadFunction(functionLoader, "glLineWidth");
        pfn_glLinkProgram = LoadFunction(functionLoader, "glLinkProgram");
        pfn_glPixelStorei = LoadFunction(functionLoader, "glPixelStorei");
        pfn_glPolygonOffset = LoadFunction(functionLoader, "glPolygonOffset");
        pfn_glReadPixels = LoadFunction(functionLoader, "glReadPixels");
        pfn_glReleaseShaderCompiler = LoadFunction(functionLoader, "glReleaseShaderCompiler");
        pfn_glRenderbufferStorage = LoadFunction(functionLoader, "glRenderbufferStorage");
        pfn_glSampleCoverage = LoadFunction(functionLoader, "glSampleCoverage");
        pfn_glScissor = LoadFunction(functionLoader, "glScissor");
        pfn_glShaderBinary = LoadFunction(functionLoader, "glShaderBinary");
        pfn_glShaderSource = LoadFunction(functionLoader, "glShaderSource");
        pfn_glStencilFunc = LoadFunction(functionLoader, "glStencilFunc");
        pfn_glStencilFuncSeparate = LoadFunction(functionLoader, "glStencilFuncSeparate");
        pfn_glStencilMask = LoadFunction(functionLoader, "glStencilMask");
        pfn_glStencilMaskSeparate = LoadFunction(functionLoader, "glStencilMaskSeparate");
        pfn_glStencilOp = LoadFunction(functionLoader, "glStencilOp");
        pfn_glStencilOpSeparate = LoadFunction(functionLoader, "glStencilOpSeparate");
        pfn_glTexImage2D = LoadFunction(functionLoader, "glTexImage2D");
        pfn_glTexParameterf = LoadFunction(functionLoader, "glTexParameterf");
        pfn_glTexParameterfv = LoadFunction(functionLoader, "glTexParameterfv");
        pfn_glTexParameteri = LoadFunction(functionLoader, "glTexParameteri");
        pfn_glTexParameteriv = LoadFunction(functionLoader, "glTexParameteriv");
        pfn_glTexSubImage2D = LoadFunction(functionLoader, "glTexSubImage2D");
        pfn_glUniform1f = LoadFunction(functionLoader, "glUniform1f");
        pfn_glUniform1fv = LoadFunction(functionLoader, "glUniform1fv");
        pfn_glUniform1i = LoadFunction(functionLoader, "glUniform1i");
        pfn_glUniform1iv = LoadFunction(functionLoader, "glUniform1iv");
        pfn_glUniform2f = LoadFunction(functionLoader, "glUniform2f");
        pfn_glUniform2fv = LoadFunction(functionLoader, "glUniform2fv");
        pfn_glUniform2i = LoadFunction(functionLoader, "glUniform2i");
        pfn_glUniform2iv = LoadFunction(functionLoader, "glUniform2iv");
        pfn_glUniform3f = LoadFunction(functionLoader, "glUniform3f");
        pfn_glUniform3fv = LoadFunction(functionLoader, "glUniform3fv");
        pfn_glUniform3i = LoadFunction(functionLoader, "glUniform3i");
        pfn_glUniform3iv = LoadFunction(functionLoader, "glUniform3iv");
        pfn_glUniform4f = LoadFunction(functionLoader, "glUniform4f");
        pfn_glUniform4fv = LoadFunction(functionLoader, "glUniform4fv");
        pfn_glUniform4i = LoadFunction(functionLoader, "glUniform4i");
        pfn_glUniform4iv = LoadFunction(functionLoader, "glUniform4iv");
        pfn_glUniformMatrix2fv = LoadFunction(functionLoader, "glUniformMatrix2fv");
        pfn_glUniformMatrix3fv = LoadFunction(functionLoader, "glUniformMatrix3fv");
        pfn_glUniformMatrix4fv = LoadFunction(functionLoader, "glUniformMatrix4fv");
        pfn_glUseProgram = LoadFunction(functionLoader, "glUseProgram");
        pfn_glValidateProgram = LoadFunction(functionLoader, "glValidateProgram");
        pfn_glVertexAttrib1f = LoadFunction(functionLoader, "glVertexAttrib1f");
        pfn_glVertexAttrib1fv = LoadFunction(functionLoader, "glVertexAttrib1fv");
        pfn_glVertexAttrib2f = LoadFunction(functionLoader, "glVertexAttrib2f");
        pfn_glVertexAttrib2fv = LoadFunction(functionLoader, "glVertexAttrib2fv");
        pfn_glVertexAttrib3f = LoadFunction(functionLoader, "glVertexAttrib3f");
        pfn_glVertexAttrib3fv = LoadFunction(functionLoader, "glVertexAttrib3fv");
        pfn_glVertexAttrib4f = LoadFunction(functionLoader, "glVertexAttrib4f");
        pfn_glVertexAttrib4fv = LoadFunction(functionLoader, "glVertexAttrib4fv");
        pfn_glVertexAttribPointer = LoadFunction(functionLoader, "glVertexAttribPointer");
        pfn_glViewport = LoadFunction(functionLoader, "glViewport");
        pfn_glReadBuffer = LoadFunction(functionLoader, "glReadBuffer");
        pfn_glDrawRangeElements = LoadFunction(functionLoader, "glDrawRangeElements");
        pfn_glTexImage3D = LoadFunction(functionLoader, "glTexImage3D");
        pfn_glTexSubImage3D = LoadFunction(functionLoader, "glTexSubImage3D");
        pfn_glCopyTexSubImage3D = LoadFunction(functionLoader, "glCopyTexSubImage3D");
        pfn_glCompressedTexImage3D = LoadFunction(functionLoader, "glCompressedTexImage3D");
        pfn_glCompressedTexSubImage3D = LoadFunction(functionLoader, "glCompressedTexSubImage3D");
        pfn_glGenQueries = LoadFunction(functionLoader, "glGenQueries");
        pfn_glDeleteQueries = LoadFunction(functionLoader, "glDeleteQueries");
        pfn_glIsQuery = LoadFunction(functionLoader, "glIsQuery");
        pfn_glBeginQuery = LoadFunction(functionLoader, "glBeginQuery");
        pfn_glEndQuery = LoadFunction(functionLoader, "glEndQuery");
        pfn_glGetQueryiv = LoadFunction(functionLoader, "glGetQueryiv");
        pfn_glGetQueryObjectuiv = LoadFunction(functionLoader, "glGetQueryObjectuiv");
        pfn_glUnmapBuffer = LoadFunction(functionLoader, "glUnmapBuffer");
        pfn_glGetBufferPointerv = LoadFunction(functionLoader, "glGetBufferPointerv");
        pfn_glDrawBuffers = LoadFunction(functionLoader, "glDrawBuffers");
        pfn_glUniformMatrix2x3fv = LoadFunction(functionLoader, "glUniformMatrix2x3fv");
        pfn_glUniformMatrix3x2fv = LoadFunction(functionLoader, "glUniformMatrix3x2fv");
        pfn_glUniformMatrix2x4fv = LoadFunction(functionLoader, "glUniformMatrix2x4fv");
        pfn_glUniformMatrix4x2fv = LoadFunction(functionLoader, "glUniformMatrix4x2fv");
        pfn_glUniformMatrix3x4fv = LoadFunction(functionLoader, "glUniformMatrix3x4fv");
        pfn_glUniformMatrix4x3fv = LoadFunction(functionLoader, "glUniformMatrix4x3fv");
        pfn_glBlitFramebuffer = LoadFunction(functionLoader, "glBlitFramebuffer");
        pfn_glRenderbufferStorageMultisample = LoadFunction(functionLoader, "glRenderbufferStorageMultisample");
        pfn_glFramebufferTextureLayer = LoadFunction(functionLoader, "glFramebufferTextureLayer");
        pfn_glMapBufferRange = LoadFunction(functionLoader, "glMapBufferRange");
        pfn_glFlushMappedBufferRange = LoadFunction(functionLoader, "glFlushMappedBufferRange");
        pfn_glBindVertexArray = LoadFunction(functionLoader, "glBindVertexArray");
        pfn_glDeleteVertexArrays = LoadFunction(functionLoader, "glDeleteVertexArrays");
        pfn_glGenVertexArrays = LoadFunction(functionLoader, "glGenVertexArrays");
        pfn_glIsVertexArray = LoadFunction(functionLoader, "glIsVertexArray");
        pfn_glGetIntegeri_v = LoadFunction(functionLoader, "glGetIntegeri_v");
        pfn_glBeginTransformFeedback = LoadFunction(functionLoader, "glBeginTransformFeedback");
        pfn_glEndTransformFeedback = LoadFunction(functionLoader, "glEndTransformFeedback");
        pfn_glBindBufferRange = LoadFunction(functionLoader, "glBindBufferRange");
        pfn_glBindBufferBase = LoadFunction(functionLoader, "glBindBufferBase");
        pfn_glTransformFeedbackVaryings = LoadFunction(functionLoader, "glTransformFeedbackVaryings");
        pfn_glGetTransformFeedbackVarying = LoadFunction(functionLoader, "glGetTransformFeedbackVarying");
        pfn_glVertexAttribIPointer = LoadFunction(functionLoader, "glVertexAttribIPointer");
        pfn_glGetVertexAttribIiv = LoadFunction(functionLoader, "glGetVertexAttribIiv");
        pfn_glGetVertexAttribIuiv = LoadFunction(functionLoader, "glGetVertexAttribIuiv");
        pfn_glVertexAttribI4i = LoadFunction(functionLoader, "glVertexAttribI4i");
        pfn_glVertexAttribI4ui = LoadFunction(functionLoader, "glVertexAttribI4ui");
        pfn_glVertexAttribI4iv = LoadFunction(functionLoader, "glVertexAttribI4iv");
        pfn_glVertexAttribI4uiv = LoadFunction(functionLoader, "glVertexAttribI4uiv");
        pfn_glGetUniformuiv = LoadFunction(functionLoader, "glGetUniformuiv");
        pfn_glGetFragDataLocation = LoadFunction(functionLoader, "glGetFragDataLocation");
        pfn_glUniform1ui = LoadFunction(functionLoader, "glUniform1ui");
        pfn_glUniform2ui = LoadFunction(functionLoader, "glUniform2ui");
        pfn_glUniform3ui = LoadFunction(functionLoader, "glUniform3ui");
        pfn_glUniform4ui = LoadFunction(functionLoader, "glUniform4ui");
        pfn_glUniform1uiv = LoadFunction(functionLoader, "glUniform1uiv");
        pfn_glUniform2uiv = LoadFunction(functionLoader, "glUniform2uiv");
        pfn_glUniform3uiv = LoadFunction(functionLoader, "glUniform3uiv");
        pfn_glUniform4uiv = LoadFunction(functionLoader, "glUniform4uiv");
        pfn_glClearBufferiv = LoadFunction(functionLoader, "glClearBufferiv");
        pfn_glClearBufferuiv = LoadFunction(functionLoader, "glClearBufferuiv");
        pfn_glClearBufferfv = LoadFunction(functionLoader, "glClearBufferfv");
        pfn_glClearBufferfi = LoadFunction(functionLoader, "glClearBufferfi");
        pfn_glGetStringi = LoadFunction(functionLoader, "glGetStringi");
        pfn_glCopyBufferSubData = LoadFunction(functionLoader, "glCopyBufferSubData");
        pfn_glGetUniformIndices = LoadFunction(functionLoader, "glGetUniformIndices");
        pfn_glGetActiveUniformsiv = LoadFunction(functionLoader, "glGetActiveUniformsiv");
        pfn_glGetUniformBlockIndex = LoadFunction(functionLoader, "glGetUniformBlockIndex");
        pfn_glGetActiveUniformBlockiv = LoadFunction(functionLoader, "glGetActiveUniformBlockiv");
        pfn_glGetActiveUniformBlockName = LoadFunction(functionLoader, "glGetActiveUniformBlockName");
        pfn_glUniformBlockBinding = LoadFunction(functionLoader, "glUniformBlockBinding");
        pfn_glDrawArraysInstanced = LoadFunction(functionLoader, "glDrawArraysInstanced");
        pfn_glDrawElementsInstanced = LoadFunction(functionLoader, "glDrawElementsInstanced");
        pfn_glFenceSync = LoadFunction(functionLoader, "glFenceSync");
        pfn_glIsSync = LoadFunction(functionLoader, "glIsSync");
        pfn_glDeleteSync = LoadFunction(functionLoader, "glDeleteSync");
        pfn_glClientWaitSync = LoadFunction(functionLoader, "glClientWaitSync");
        pfn_glWaitSync = LoadFunction(functionLoader, "glWaitSync");
        pfn_glGetInteger64v = LoadFunction(functionLoader, "glGetInteger64v");
        pfn_glGetSynciv = LoadFunction(functionLoader, "glGetSynciv");
        pfn_glGetInteger64i_v = LoadFunction(functionLoader, "glGetInteger64i_v");
        pfn_glGetBufferParameteri64v = LoadFunction(functionLoader, "glGetBufferParameteri64v");
        pfn_glGenSamplers = LoadFunction(functionLoader, "glGenSamplers");
        pfn_glDeleteSamplers = LoadFunction(functionLoader, "glDeleteSamplers");
        pfn_glIsSampler = LoadFunction(functionLoader, "glIsSampler");
        pfn_glBindSampler = LoadFunction(functionLoader, "glBindSampler");
        pfn_glSamplerParameteri = LoadFunction(functionLoader, "glSamplerParameteri");
        pfn_glSamplerParameteriv = LoadFunction(functionLoader, "glSamplerParameteriv");
        pfn_glSamplerParameterf = LoadFunction(functionLoader, "glSamplerParameterf");
        pfn_glSamplerParameterfv = LoadFunction(functionLoader, "glSamplerParameterfv");
        pfn_glGetSamplerParameteriv = LoadFunction(functionLoader, "glGetSamplerParameteriv");
        pfn_glGetSamplerParameterfv = LoadFunction(functionLoader, "glGetSamplerParameterfv");
        pfn_glVertexAttribDivisor = LoadFunction(functionLoader, "glVertexAttribDivisor");
        pfn_glBindTransformFeedback = LoadFunction(functionLoader, "glBindTransformFeedback");
        pfn_glDeleteTransformFeedbacks = LoadFunction(functionLoader, "glDeleteTransformFeedbacks");
        pfn_glGenTransformFeedbacks = LoadFunction(functionLoader, "glGenTransformFeedbacks");
        pfn_glIsTransformFeedback = LoadFunction(functionLoader, "glIsTransformFeedback");
        pfn_glPauseTransformFeedback = LoadFunction(functionLoader, "glPauseTransformFeedback");
        pfn_glResumeTransformFeedback = LoadFunction(functionLoader, "glResumeTransformFeedback");
        pfn_glGetProgramBinary = LoadFunction(functionLoader, "glGetProgramBinary");
        pfn_glProgramBinary = LoadFunction(functionLoader, "glProgramBinary");
        pfn_glProgramParameteri = LoadFunction(functionLoader, "glProgramParameteri");
        pfn_glInvalidateFramebuffer = LoadFunction(functionLoader, "glInvalidateFramebuffer");
        pfn_glInvalidateSubFramebuffer = LoadFunction(functionLoader, "glInvalidateSubFramebuffer");
        pfn_glTexStorage2D = LoadFunction(functionLoader, "glTexStorage2D");
        pfn_glTexStorage3D = LoadFunction(functionLoader, "glTexStorage3D");
        pfn_glGetInternalformativ = LoadFunction(functionLoader, "glGetInternalformativ");
        pfn_glDispatchCompute = LoadFunction(functionLoader, "glDispatchCompute");
        pfn_glDispatchComputeIndirect = LoadFunction(functionLoader, "glDispatchComputeIndirect");
        pfn_glDrawArraysIndirect = LoadFunction(functionLoader, "glDrawArraysIndirect");
        pfn_glDrawElementsIndirect = LoadFunction(functionLoader, "glDrawElementsIndirect");
        pfn_glFramebufferParameteri = LoadFunction(functionLoader, "glFramebufferParameteri");
        pfn_glGetFramebufferParameteriv = LoadFunction(functionLoader, "glGetFramebufferParameteriv");
        pfn_glGetProgramInterfaceiv = LoadFunction(functionLoader, "glGetProgramInterfaceiv");
        pfn_glGetProgramResourceIndex = LoadFunction(functionLoader, "glGetProgramResourceIndex");
        pfn_glGetProgramResourceName = LoadFunction(functionLoader, "glGetProgramResourceName");
        pfn_glGetProgramResourceiv = LoadFunction(functionLoader, "glGetProgramResourceiv");
        pfn_glGetProgramResourceLocation = LoadFunction(functionLoader, "glGetProgramResourceLocation");
        pfn_glUseProgramStages = LoadFunction(functionLoader, "glUseProgramStages");
        pfn_glActiveShaderProgram = LoadFunction(functionLoader, "glActiveShaderProgram");
        pfn_glCreateShaderProgramv = LoadFunction(functionLoader, "glCreateShaderProgramv");
        pfn_glBindProgramPipeline = LoadFunction(functionLoader, "glBindProgramPipeline");
        pfn_glDeleteProgramPipelines = LoadFunction(functionLoader, "glDeleteProgramPipelines");
        pfn_glGenProgramPipelines = LoadFunction(functionLoader, "glGenProgramPipelines");
        pfn_glIsProgramPipeline = LoadFunction(functionLoader, "glIsProgramPipeline");
        pfn_glGetProgramPipelineiv = LoadFunction(functionLoader, "glGetProgramPipelineiv");
        pfn_glProgramUniform1i = LoadFunction(functionLoader, "glProgramUniform1i");
        pfn_glProgramUniform2i = LoadFunction(functionLoader, "glProgramUniform2i");
        pfn_glProgramUniform3i = LoadFunction(functionLoader, "glProgramUniform3i");
        pfn_glProgramUniform4i = LoadFunction(functionLoader, "glProgramUniform4i");
        pfn_glProgramUniform1ui = LoadFunction(functionLoader, "glProgramUniform1ui");
        pfn_glProgramUniform2ui = LoadFunction(functionLoader, "glProgramUniform2ui");
        pfn_glProgramUniform3ui = LoadFunction(functionLoader, "glProgramUniform3ui");
        pfn_glProgramUniform4ui = LoadFunction(functionLoader, "glProgramUniform4ui");
        pfn_glProgramUniform1f = LoadFunction(functionLoader, "glProgramUniform1f");
        pfn_glProgramUniform2f = LoadFunction(functionLoader, "glProgramUniform2f");
        pfn_glProgramUniform3f = LoadFunction(functionLoader, "glProgramUniform3f");
        pfn_glProgramUniform4f = LoadFunction(functionLoader, "glProgramUniform4f");
        pfn_glProgramUniform1iv = LoadFunction(functionLoader, "glProgramUniform1iv");
        pfn_glProgramUniform2iv = LoadFunction(functionLoader, "glProgramUniform2iv");
        pfn_glProgramUniform3iv = LoadFunction(functionLoader, "glProgramUniform3iv");
        pfn_glProgramUniform4iv = LoadFunction(functionLoader, "glProgramUniform4iv");
        pfn_glProgramUniform1uiv = LoadFunction(functionLoader, "glProgramUniform1uiv");
        pfn_glProgramUniform2uiv = LoadFunction(functionLoader, "glProgramUniform2uiv");
        pfn_glProgramUniform3uiv = LoadFunction(functionLoader, "glProgramUniform3uiv");
        pfn_glProgramUniform4uiv = LoadFunction(functionLoader, "glProgramUniform4uiv");
        pfn_glProgramUniform1fv = LoadFunction(functionLoader, "glProgramUniform1fv");
        pfn_glProgramUniform2fv = LoadFunction(functionLoader, "glProgramUniform2fv");
        pfn_glProgramUniform3fv = LoadFunction(functionLoader, "glProgramUniform3fv");
        pfn_glProgramUniform4fv = LoadFunction(functionLoader, "glProgramUniform4fv");
        pfn_glProgramUniformMatrix2fv = LoadFunction(functionLoader, "glProgramUniformMatrix2fv");
        pfn_glProgramUniformMatrix3fv = LoadFunction(functionLoader, "glProgramUniformMatrix3fv");
        pfn_glProgramUniformMatrix4fv = LoadFunction(functionLoader, "glProgramUniformMatrix4fv");
        pfn_glProgramUniformMatrix2x3fv = LoadFunction(functionLoader, "glProgramUniformMatrix2x3fv");
        pfn_glProgramUniformMatrix3x2fv = LoadFunction(functionLoader, "glProgramUniformMatrix3x2fv");
        pfn_glProgramUniformMatrix2x4fv = LoadFunction(functionLoader, "glProgramUniformMatrix2x4fv");
        pfn_glProgramUniformMatrix4x2fv = LoadFunction(functionLoader, "glProgramUniformMatrix4x2fv");
        pfn_glProgramUniformMatrix3x4fv = LoadFunction(functionLoader, "glProgramUniformMatrix3x4fv");
        pfn_glProgramUniformMatrix4x3fv = LoadFunction(functionLoader, "glProgramUniformMatrix4x3fv");
        pfn_glValidateProgramPipeline = LoadFunction(functionLoader, "glValidateProgramPipeline");
        pfn_glGetProgramPipelineInfoLog = LoadFunction(functionLoader, "glGetProgramPipelineInfoLog");
        pfn_glBindImageTexture = LoadFunction(functionLoader, "glBindImageTexture");
        pfn_glGetBooleani_v = LoadFunction(functionLoader, "glGetBooleani_v");
        pfn_glMemoryBarrier = LoadFunction(functionLoader, "glMemoryBarrier");
        pfn_glMemoryBarrierByRegion = LoadFunction(functionLoader, "glMemoryBarrierByRegion");
        pfn_glTexStorage2DMultisample = LoadFunction(functionLoader, "glTexStorage2DMultisample");
        pfn_glGetMultisamplefv = LoadFunction(functionLoader, "glGetMultisamplefv");
        pfn_glSampleMaski = LoadFunction(functionLoader, "glSampleMaski");
        pfn_glGetTexLevelParameteriv = LoadFunction(functionLoader, "glGetTexLevelParameteriv");
        pfn_glGetTexLevelParameterfv = LoadFunction(functionLoader, "glGetTexLevelParameterfv");
        pfn_glBindVertexBuffer = LoadFunction(functionLoader, "glBindVertexBuffer");
        pfn_glVertexAttribFormat = LoadFunction(functionLoader, "glVertexAttribFormat");
        pfn_glVertexAttribIFormat = LoadFunction(functionLoader, "glVertexAttribIFormat");
        pfn_glVertexAttribBinding = LoadFunction(functionLoader, "glVertexAttribBinding");
        pfn_glVertexBindingDivisor = LoadFunction(functionLoader, "glVertexBindingDivisor");
        pfn_glBlendBarrier = LoadFunction(functionLoader, "glBlendBarrier");
        pfn_glCopyImageSubData = LoadFunction(functionLoader, "glCopyImageSubData");
        pfn_glDebugMessageControl = LoadFunction(functionLoader, "glDebugMessageControl");
        pfn_glDebugMessageInsert = LoadFunction(functionLoader, "glDebugMessageInsert");
        pfn_glDebugMessageCallback = LoadFunction(functionLoader, "glDebugMessageCallback");
        pfn_glGetDebugMessageLog = LoadFunction(functionLoader, "glGetDebugMessageLog");
        pfn_glPushDebugGroup = LoadFunction(functionLoader, "glPushDebugGroup");
        pfn_glPopDebugGroup = LoadFunction(functionLoader, "glPopDebugGroup");
        pfn_glObjectLabel = LoadFunction(functionLoader, "glObjectLabel");
        pfn_glGetObjectLabel = LoadFunction(functionLoader, "glGetObjectLabel");
        pfn_glObjectPtrLabel = LoadFunction(functionLoader, "glObjectPtrLabel");
        pfn_glGetObjectPtrLabel = LoadFunction(functionLoader, "glGetObjectPtrLabel");
        pfn_glGetPointerv = LoadFunction(functionLoader, "glGetPointerv");
        pfn_glEnablei = LoadFunction(functionLoader, "glEnablei");
        pfn_glDisablei = LoadFunction(functionLoader, "glDisablei");
        pfn_glBlendEquationi = LoadFunction(functionLoader, "glBlendEquationi");
        pfn_glBlendEquationSeparatei = LoadFunction(functionLoader, "glBlendEquationSeparatei");
        pfn_glBlendFunci = LoadFunction(functionLoader, "glBlendFunci");
        pfn_glBlendFuncSeparatei = LoadFunction(functionLoader, "glBlendFuncSeparatei");
        pfn_glColorMaski = LoadFunction(functionLoader, "glColorMaski");
        pfn_glIsEnabledi = LoadFunction(functionLoader, "glIsEnabledi");
        pfn_glDrawElementsBaseVertex = LoadFunction(functionLoader, "glDrawElementsBaseVertex");
        pfn_glDrawRangeElementsBaseVertex = LoadFunction(functionLoader, "glDrawRangeElementsBaseVertex");
        pfn_glDrawElementsInstancedBaseVertex = LoadFunction(functionLoader, "glDrawElementsInstancedBaseVertex");
        pfn_glFramebufferTexture = LoadFunction(functionLoader, "glFramebufferTexture");
        pfn_glPrimitiveBoundingBox = LoadFunction(functionLoader, "glPrimitiveBoundingBox");
        pfn_glGetGraphicsResetStatus = LoadFunction(functionLoader, "glGetGraphicsResetStatus");
        pfn_glReadnPixels = LoadFunction(functionLoader, "glReadnPixels");
        pfn_glGetnUniformfv = LoadFunction(functionLoader, "glGetnUniformfv");
        pfn_glGetnUniformiv = LoadFunction(functionLoader, "glGetnUniformiv");
        pfn_glGetnUniformuiv = LoadFunction(functionLoader, "glGetnUniformuiv");
        pfn_glMinSampleShading = LoadFunction(functionLoader, "glMinSampleShading");
        pfn_glPatchParameteri = LoadFunction(functionLoader, "glPatchParameteri");
        pfn_glTexParameterIiv = LoadFunction(functionLoader, "glTexParameterIiv");
        pfn_glTexParameterIuiv = LoadFunction(functionLoader, "glTexParameterIuiv");
        pfn_glGetTexParameterIiv = LoadFunction(functionLoader, "glGetTexParameterIiv");
        pfn_glGetTexParameterIuiv = LoadFunction(functionLoader, "glGetTexParameterIuiv");
        pfn_glSamplerParameterIiv = LoadFunction(functionLoader, "glSamplerParameterIiv");
        pfn_glSamplerParameterIuiv = LoadFunction(functionLoader, "glSamplerParameterIuiv");
        pfn_glGetSamplerParameterIiv = LoadFunction(functionLoader, "glGetSamplerParameterIiv");
        pfn_glGetSamplerParameterIuiv = LoadFunction(functionLoader, "glGetSamplerParameterIuiv");
        pfn_glTexBuffer = LoadFunction(functionLoader, "glTexBuffer");
        pfn_glTexBufferRange = LoadFunction(functionLoader, "glTexBufferRange");
        pfn_glTexStorage3DMultisample = LoadFunction(functionLoader, "glTexStorage3DMultisample");
    }
    public const uint GL_GLES_PROTOTYPES = 1;
    public const uint GL_ES_VERSION_2_0 = 1;
    public const uint GL_DEPTH_BUFFER_BIT = 0x00000100;
    public const uint GL_STENCIL_BUFFER_BIT = 0x00000400;
    public const uint GL_COLOR_BUFFER_BIT = 0x00004000;
    public const uint GL_FALSE = 0;
    public const uint GL_TRUE = 1;
    public const uint GL_POINTS = 0x0000;
    public const uint GL_LINES = 0x0001;
    public const uint GL_LINE_LOOP = 0x0002;
    public const uint GL_LINE_STRIP = 0x0003;
    public const uint GL_TRIANGLES = 0x0004;
    public const uint GL_TRIANGLE_STRIP = 0x0005;
    public const uint GL_TRIANGLE_FAN = 0x0006;
    public const uint GL_ZERO = 0;
    public const uint GL_ONE = 1;
    public const uint GL_SRC_COLOR = 0x0300;
    public const uint GL_ONE_MINUS_SRC_COLOR = 0x0301;
    public const uint GL_SRC_ALPHA = 0x0302;
    public const uint GL_ONE_MINUS_SRC_ALPHA = 0x0303;
    public const uint GL_DST_ALPHA = 0x0304;
    public const uint GL_ONE_MINUS_DST_ALPHA = 0x0305;
    public const uint GL_DST_COLOR = 0x0306;
    public const uint GL_ONE_MINUS_DST_COLOR = 0x0307;
    public const uint GL_SRC_ALPHA_SATURATE = 0x0308;
    public const uint GL_FUNC_ADD = 0x8006;
    public const uint GL_BLEND_EQUATION = 0x8009;
    public const uint GL_BLEND_EQUATION_RGB = 0x8009;
    public const uint GL_BLEND_EQUATION_ALPHA = 0x883D;
    public const uint GL_FUNC_SUBTRACT = 0x800A;
    public const uint GL_FUNC_REVERSE_SUBTRACT = 0x800B;
    public const uint GL_BLEND_DST_RGB = 0x80C8;
    public const uint GL_BLEND_SRC_RGB = 0x80C9;
    public const uint GL_BLEND_DST_ALPHA = 0x80CA;
    public const uint GL_BLEND_SRC_ALPHA = 0x80CB;
    public const uint GL_CONSTANT_COLOR = 0x8001;
    public const uint GL_ONE_MINUS_CONSTANT_COLOR = 0x8002;
    public const uint GL_CONSTANT_ALPHA = 0x8003;
    public const uint GL_ONE_MINUS_CONSTANT_ALPHA = 0x8004;
    public const uint GL_BLEND_COLOR = 0x8005;
    public const uint GL_ARRAY_BUFFER = 0x8892;
    public const uint GL_ELEMENT_ARRAY_BUFFER = 0x8893;
    public const uint GL_ARRAY_BUFFER_BINDING = 0x8894;
    public const uint GL_ELEMENT_ARRAY_BUFFER_BINDING = 0x8895;
    public const uint GL_STREAM_DRAW = 0x88E0;
    public const uint GL_STATIC_DRAW = 0x88E4;
    public const uint GL_DYNAMIC_DRAW = 0x88E8;
    public const uint GL_BUFFER_SIZE = 0x8764;
    public const uint GL_BUFFER_USAGE = 0x8765;
    public const uint GL_CURRENT_VERTEX_ATTRIB = 0x8626;
    public const uint GL_FRONT = 0x0404;
    public const uint GL_BACK = 0x0405;
    public const uint GL_FRONT_AND_BACK = 0x0408;
    public const uint GL_TEXTURE_2D = 0x0DE1;
    public const uint GL_CULL_FACE = 0x0B44;
    public const uint GL_BLEND = 0x0BE2;
    public const uint GL_DITHER = 0x0BD0;
    public const uint GL_STENCIL_TEST = 0x0B90;
    public const uint GL_DEPTH_TEST = 0x0B71;
    public const uint GL_SCISSOR_TEST = 0x0C11;
    public const uint GL_POLYGON_OFFSET_FILL = 0x8037;
    public const uint GL_SAMPLE_ALPHA_TO_COVERAGE = 0x809E;
    public const uint GL_SAMPLE_COVERAGE = 0x80A0;
    public const uint GL_NO_ERROR = 0;
    public const uint GL_INVALID_ENUM = 0x0500;
    public const uint GL_INVALID_VALUE = 0x0501;
    public const uint GL_INVALID_OPERATION = 0x0502;
    public const uint GL_OUT_OF_MEMORY = 0x0505;
    public const uint GL_CW = 0x0900;
    public const uint GL_CCW = 0x0901;
    public const uint GL_LINE_WIDTH = 0x0B21;
    public const uint GL_ALIASED_POINT_SIZE_RANGE = 0x846D;
    public const uint GL_ALIASED_LINE_WIDTH_RANGE = 0x846E;
    public const uint GL_CULL_FACE_MODE = 0x0B45;
    public const uint GL_FRONT_FACE = 0x0B46;
    public const uint GL_DEPTH_RANGE = 0x0B70;
    public const uint GL_DEPTH_WRITEMASK = 0x0B72;
    public const uint GL_DEPTH_CLEAR_VALUE = 0x0B73;
    public const uint GL_DEPTH_FUNC = 0x0B74;
    public const uint GL_STENCIL_CLEAR_VALUE = 0x0B91;
    public const uint GL_STENCIL_FUNC = 0x0B92;
    public const uint GL_STENCIL_FAIL = 0x0B94;
    public const uint GL_STENCIL_PASS_DEPTH_FAIL = 0x0B95;
    public const uint GL_STENCIL_PASS_DEPTH_PASS = 0x0B96;
    public const uint GL_STENCIL_REF = 0x0B97;
    public const uint GL_STENCIL_VALUE_MASK = 0x0B93;
    public const uint GL_STENCIL_WRITEMASK = 0x0B98;
    public const uint GL_STENCIL_BACK_FUNC = 0x8800;
    public const uint GL_STENCIL_BACK_FAIL = 0x8801;
    public const uint GL_STENCIL_BACK_PASS_DEPTH_FAIL = 0x8802;
    public const uint GL_STENCIL_BACK_PASS_DEPTH_PASS = 0x8803;
    public const uint GL_STENCIL_BACK_REF = 0x8CA3;
    public const uint GL_STENCIL_BACK_VALUE_MASK = 0x8CA4;
    public const uint GL_STENCIL_BACK_WRITEMASK = 0x8CA5;
    public const uint GL_VIEWPORT = 0x0BA2;
    public const uint GL_SCISSOR_BOX = 0x0C10;
    public const uint GL_COLOR_CLEAR_VALUE = 0x0C22;
    public const uint GL_COLOR_WRITEMASK = 0x0C23;
    public const uint GL_UNPACK_ALIGNMENT = 0x0CF5;
    public const uint GL_PACK_ALIGNMENT = 0x0D05;
    public const uint GL_MAX_TEXTURE_SIZE = 0x0D33;
    public const uint GL_MAX_VIEWPORT_DIMS = 0x0D3A;
    public const uint GL_SUBPIXEL_BITS = 0x0D50;
    public const uint GL_RED_BITS = 0x0D52;
    public const uint GL_GREEN_BITS = 0x0D53;
    public const uint GL_BLUE_BITS = 0x0D54;
    public const uint GL_ALPHA_BITS = 0x0D55;
    public const uint GL_DEPTH_BITS = 0x0D56;
    public const uint GL_STENCIL_BITS = 0x0D57;
    public const uint GL_POLYGON_OFFSET_UNITS = 0x2A00;
    public const uint GL_POLYGON_OFFSET_FACTOR = 0x8038;
    public const uint GL_TEXTURE_BINDING_2D = 0x8069;
    public const uint GL_SAMPLE_BUFFERS = 0x80A8;
    public const uint GL_SAMPLES = 0x80A9;
    public const uint GL_SAMPLE_COVERAGE_VALUE = 0x80AA;
    public const uint GL_SAMPLE_COVERAGE_INVERT = 0x80AB;
    public const uint GL_NUM_COMPRESSED_TEXTURE_FORMATS = 0x86A2;
    public const uint GL_COMPRESSED_TEXTURE_FORMATS = 0x86A3;
    public const uint GL_DONT_CARE = 0x1100;
    public const uint GL_FASTEST = 0x1101;
    public const uint GL_NICEST = 0x1102;
    public const uint GL_GENERATE_MIPMAP_HINT = 0x8192;
    public const uint GL_BYTE = 0x1400;
    public const uint GL_UNSIGNED_BYTE = 0x1401;
    public const uint GL_SHORT = 0x1402;
    public const uint GL_UNSIGNED_SHORT = 0x1403;
    public const uint GL_INT = 0x1404;
    public const uint GL_UNSIGNED_INT = 0x1405;
    public const uint GL_FLOAT = 0x1406;
    public const uint GL_FIXED = 0x140C;
    public const uint GL_DEPTH_COMPONENT = 0x1902;
    public const uint GL_ALPHA = 0x1906;
    public const uint GL_RGB = 0x1907;
    public const uint GL_RGBA = 0x1908;
    public const uint GL_LUMINANCE = 0x1909;
    public const uint GL_LUMINANCE_ALPHA = 0x190A;
    public const uint GL_UNSIGNED_SHORT_4_4_4_4 = 0x8033;
    public const uint GL_UNSIGNED_SHORT_5_5_5_1 = 0x8034;
    public const uint GL_UNSIGNED_SHORT_5_6_5 = 0x8363;
    public const uint GL_FRAGMENT_SHADER = 0x8B30;
    public const uint GL_VERTEX_SHADER = 0x8B31;
    public const uint GL_MAX_VERTEX_ATTRIBS = 0x8869;
    public const uint GL_MAX_VERTEX_UNIFORM_VECTORS = 0x8DFB;
    public const uint GL_MAX_VARYING_VECTORS = 0x8DFC;
    public const uint GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS = 0x8B4D;
    public const uint GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS = 0x8B4C;
    public const uint GL_MAX_TEXTURE_IMAGE_UNITS = 0x8872;
    public const uint GL_MAX_FRAGMENT_UNIFORM_VECTORS = 0x8DFD;
    public const uint GL_SHADER_TYPE = 0x8B4F;
    public const uint GL_DELETE_STATUS = 0x8B80;
    public const uint GL_LINK_STATUS = 0x8B82;
    public const uint GL_VALIDATE_STATUS = 0x8B83;
    public const uint GL_ATTACHED_SHADERS = 0x8B85;
    public const uint GL_ACTIVE_UNIFORMS = 0x8B86;
    public const uint GL_ACTIVE_UNIFORM_MAX_LENGTH = 0x8B87;
    public const uint GL_ACTIVE_ATTRIBUTES = 0x8B89;
    public const uint GL_ACTIVE_ATTRIBUTE_MAX_LENGTH = 0x8B8A;
    public const uint GL_SHADING_LANGUAGE_VERSION = 0x8B8C;
    public const uint GL_CURRENT_PROGRAM = 0x8B8D;
    public const uint GL_NEVER = 0x0200;
    public const uint GL_LESS = 0x0201;
    public const uint GL_EQUAL = 0x0202;
    public const uint GL_LEQUAL = 0x0203;
    public const uint GL_GREATER = 0x0204;
    public const uint GL_NOTEQUAL = 0x0205;
    public const uint GL_GEQUAL = 0x0206;
    public const uint GL_ALWAYS = 0x0207;
    public const uint GL_KEEP = 0x1E00;
    public const uint GL_REPLACE = 0x1E01;
    public const uint GL_INCR = 0x1E02;
    public const uint GL_DECR = 0x1E03;
    public const uint GL_INVERT = 0x150A;
    public const uint GL_INCR_WRAP = 0x8507;
    public const uint GL_DECR_WRAP = 0x8508;
    public const uint GL_VENDOR = 0x1F00;
    public const uint GL_RENDERER = 0x1F01;
    public const uint GL_VERSION = 0x1F02;
    public const uint GL_NEAREST = 0x2600;
    public const uint GL_LINEAR = 0x2601;
    public const uint GL_NEAREST_MIPMAP_NEAREST = 0x2700;
    public const uint GL_LINEAR_MIPMAP_NEAREST = 0x2701;
    public const uint GL_NEAREST_MIPMAP_LINEAR = 0x2702;
    public const uint GL_LINEAR_MIPMAP_LINEAR = 0x2703;
    public const uint GL_TEXTURE_MAG_FILTER = 0x2800;
    public const uint GL_TEXTURE_MIN_FILTER = 0x2801;
    public const uint GL_TEXTURE_WRAP_S = 0x2802;
    public const uint GL_TEXTURE_WRAP_T = 0x2803;
    public const uint GL_TEXTURE = 0x1702;
    public const uint GL_TEXTURE_CUBE_MAP = 0x8513;
    public const uint GL_TEXTURE_BINDING_CUBE_MAP = 0x8514;
    public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515;
    public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516;
    public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517;
    public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518;
    public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519;
    public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A;
    public const uint GL_MAX_CUBE_MAP_TEXTURE_SIZE = 0x851C;
    public const uint GL_TEXTURE0 = 0x84C0;
    public const uint GL_TEXTURE1 = 0x84C1;
    public const uint GL_TEXTURE2 = 0x84C2;
    public const uint GL_TEXTURE3 = 0x84C3;
    public const uint GL_TEXTURE4 = 0x84C4;
    public const uint GL_TEXTURE5 = 0x84C5;
    public const uint GL_TEXTURE6 = 0x84C6;
    public const uint GL_TEXTURE7 = 0x84C7;
    public const uint GL_TEXTURE8 = 0x84C8;
    public const uint GL_TEXTURE9 = 0x84C9;
    public const uint GL_TEXTURE10 = 0x84CA;
    public const uint GL_TEXTURE11 = 0x84CB;
    public const uint GL_TEXTURE12 = 0x84CC;
    public const uint GL_TEXTURE13 = 0x84CD;
    public const uint GL_TEXTURE14 = 0x84CE;
    public const uint GL_TEXTURE15 = 0x84CF;
    public const uint GL_TEXTURE16 = 0x84D0;
    public const uint GL_TEXTURE17 = 0x84D1;
    public const uint GL_TEXTURE18 = 0x84D2;
    public const uint GL_TEXTURE19 = 0x84D3;
    public const uint GL_TEXTURE20 = 0x84D4;
    public const uint GL_TEXTURE21 = 0x84D5;
    public const uint GL_TEXTURE22 = 0x84D6;
    public const uint GL_TEXTURE23 = 0x84D7;
    public const uint GL_TEXTURE24 = 0x84D8;
    public const uint GL_TEXTURE25 = 0x84D9;
    public const uint GL_TEXTURE26 = 0x84DA;
    public const uint GL_TEXTURE27 = 0x84DB;
    public const uint GL_TEXTURE28 = 0x84DC;
    public const uint GL_TEXTURE29 = 0x84DD;
    public const uint GL_TEXTURE30 = 0x84DE;
    public const uint GL_TEXTURE31 = 0x84DF;
    public const uint GL_ACTIVE_TEXTURE = 0x84E0;
    public const uint GL_REPEAT = 0x2901;
    public const uint GL_CLAMP_TO_EDGE = 0x812F;
    public const uint GL_MIRRORED_REPEAT = 0x8370;
    public const uint GL_FLOAT_VEC2 = 0x8B50;
    public const uint GL_FLOAT_VEC3 = 0x8B51;
    public const uint GL_FLOAT_VEC4 = 0x8B52;
    public const uint GL_INT_VEC2 = 0x8B53;
    public const uint GL_INT_VEC3 = 0x8B54;
    public const uint GL_INT_VEC4 = 0x8B55;
    public const uint GL_BOOL = 0x8B56;
    public const uint GL_BOOL_VEC2 = 0x8B57;
    public const uint GL_BOOL_VEC3 = 0x8B58;
    public const uint GL_BOOL_VEC4 = 0x8B59;
    public const uint GL_FLOAT_MAT2 = 0x8B5A;
    public const uint GL_FLOAT_MAT3 = 0x8B5B;
    public const uint GL_FLOAT_MAT4 = 0x8B5C;
    public const uint GL_SAMPLER_2D = 0x8B5E;
    public const uint GL_SAMPLER_CUBE = 0x8B60;
    public const uint GL_VERTEX_ATTRIB_ARRAY_ENABLED = 0x8622;
    public const uint GL_VERTEX_ATTRIB_ARRAY_SIZE = 0x8623;
    public const uint GL_VERTEX_ATTRIB_ARRAY_STRIDE = 0x8624;
    public const uint GL_VERTEX_ATTRIB_ARRAY_TYPE = 0x8625;
    public const uint GL_VERTEX_ATTRIB_ARRAY_NORMALIZED = 0x886A;
    public const uint GL_VERTEX_ATTRIB_ARRAY_POINTER = 0x8645;
    public const uint GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING = 0x889F;
    public const uint GL_IMPLEMENTATION_COLOR_READ_TYPE = 0x8B9A;
    public const uint GL_IMPLEMENTATION_COLOR_READ_FORMAT = 0x8B9B;
    public const uint GL_COMPILE_STATUS = 0x8B81;
    public const uint GL_INFO_LOG_LENGTH = 0x8B84;
    public const uint GL_SHADER_SOURCE_LENGTH = 0x8B88;
    public const uint GL_SHADER_COMPILER = 0x8DFA;
    public const uint GL_SHADER_BINARY_FORMATS = 0x8DF8;
    public const uint GL_NUM_SHADER_BINARY_FORMATS = 0x8DF9;
    public const uint GL_LOW_FLOAT = 0x8DF0;
    public const uint GL_MEDIUM_FLOAT = 0x8DF1;
    public const uint GL_HIGH_FLOAT = 0x8DF2;
    public const uint GL_LOW_INT = 0x8DF3;
    public const uint GL_MEDIUM_INT = 0x8DF4;
    public const uint GL_HIGH_INT = 0x8DF5;
    public const uint GL_FRAMEBUFFER = 0x8D40;
    public const uint GL_RENDERBUFFER = 0x8D41;
    public const uint GL_RGBA4 = 0x8056;
    public const uint GL_RGB5_A1 = 0x8057;
    public const uint GL_RGB565 = 0x8D62;
    public const uint GL_DEPTH_COMPONENT16 = 0x81A5;
    public const uint GL_STENCIL_INDEX8 = 0x8D48;
    public const uint GL_RENDERBUFFER_WIDTH = 0x8D42;
    public const uint GL_RENDERBUFFER_HEIGHT = 0x8D43;
    public const uint GL_RENDERBUFFER_INTERNAL_FORMAT = 0x8D44;
    public const uint GL_RENDERBUFFER_RED_SIZE = 0x8D50;
    public const uint GL_RENDERBUFFER_GREEN_SIZE = 0x8D51;
    public const uint GL_RENDERBUFFER_BLUE_SIZE = 0x8D52;
    public const uint GL_RENDERBUFFER_ALPHA_SIZE = 0x8D53;
    public const uint GL_RENDERBUFFER_DEPTH_SIZE = 0x8D54;
    public const uint GL_RENDERBUFFER_STENCIL_SIZE = 0x8D55;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = 0x8CD0;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = 0x8CD1;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = 0x8CD2;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x8CD3;
    public const uint GL_COLOR_ATTACHMENT0 = 0x8CE0;
    public const uint GL_DEPTH_ATTACHMENT = 0x8D00;
    public const uint GL_STENCIL_ATTACHMENT = 0x8D20;
    public const uint GL_NONE = 0;
    public const uint GL_FRAMEBUFFER_COMPLETE = 0x8CD5;
    public const uint GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x8CD6;
    public const uint GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x8CD7;
    public const uint GL_FRAMEBUFFER_INCOMPLETE_DIMENSIONS = 0x8CD9;
    public const uint GL_FRAMEBUFFER_UNSUPPORTED = 0x8CDD;
    public const uint GL_FRAMEBUFFER_BINDING = 0x8CA6;
    public const uint GL_RENDERBUFFER_BINDING = 0x8CA7;
    public const uint GL_MAX_RENDERBUFFER_SIZE = 0x84E8;
    public const uint GL_INVALID_FRAMEBUFFER_OPERATION = 0x0506;
    public const uint GL_ES_VERSION_3_0 = 1;
    public const uint GL_READ_BUFFER = 0x0C02;
    public const uint GL_UNPACK_ROW_LENGTH = 0x0CF2;
    public const uint GL_UNPACK_SKIP_ROWS = 0x0CF3;
    public const uint GL_UNPACK_SKIP_PIXELS = 0x0CF4;
    public const uint GL_PACK_ROW_LENGTH = 0x0D02;
    public const uint GL_PACK_SKIP_ROWS = 0x0D03;
    public const uint GL_PACK_SKIP_PIXELS = 0x0D04;
    public const uint GL_COLOR = 0x1800;
    public const uint GL_DEPTH = 0x1801;
    public const uint GL_STENCIL = 0x1802;
    public const uint GL_RED = 0x1903;
    public const uint GL_RGB8 = 0x8051;
    public const uint GL_RGBA8 = 0x8058;
    public const uint GL_RGB10_A2 = 0x8059;
    public const uint GL_TEXTURE_BINDING_3D = 0x806A;
    public const uint GL_UNPACK_SKIP_IMAGES = 0x806D;
    public const uint GL_UNPACK_IMAGE_HEIGHT = 0x806E;
    public const uint GL_TEXTURE_3D = 0x806F;
    public const uint GL_TEXTURE_WRAP_R = 0x8072;
    public const uint GL_MAX_3D_TEXTURE_SIZE = 0x8073;
    public const uint GL_UNSIGNED_INT_2_10_10_10_REV = 0x8368;
    public const uint GL_MAX_ELEMENTS_VERTICES = 0x80E8;
    public const uint GL_MAX_ELEMENTS_INDICES = 0x80E9;
    public const uint GL_TEXTURE_MIN_LOD = 0x813A;
    public const uint GL_TEXTURE_MAX_LOD = 0x813B;
    public const uint GL_TEXTURE_BASE_LEVEL = 0x813C;
    public const uint GL_TEXTURE_MAX_LEVEL = 0x813D;
    public const uint GL_MIN = 0x8007;
    public const uint GL_MAX = 0x8008;
    public const uint GL_DEPTH_COMPONENT24 = 0x81A6;
    public const uint GL_MAX_TEXTURE_LOD_BIAS = 0x84FD;
    public const uint GL_TEXTURE_COMPARE_MODE = 0x884C;
    public const uint GL_TEXTURE_COMPARE_FUNC = 0x884D;
    public const uint GL_CURRENT_QUERY = 0x8865;
    public const uint GL_QUERY_RESULT = 0x8866;
    public const uint GL_QUERY_RESULT_AVAILABLE = 0x8867;
    public const uint GL_BUFFER_MAPPED = 0x88BC;
    public const uint GL_BUFFER_MAP_POINTER = 0x88BD;
    public const uint GL_STREAM_READ = 0x88E1;
    public const uint GL_STREAM_COPY = 0x88E2;
    public const uint GL_STATIC_READ = 0x88E5;
    public const uint GL_STATIC_COPY = 0x88E6;
    public const uint GL_DYNAMIC_READ = 0x88E9;
    public const uint GL_DYNAMIC_COPY = 0x88EA;
    public const uint GL_MAX_DRAW_BUFFERS = 0x8824;
    public const uint GL_DRAW_BUFFER0 = 0x8825;
    public const uint GL_DRAW_BUFFER1 = 0x8826;
    public const uint GL_DRAW_BUFFER2 = 0x8827;
    public const uint GL_DRAW_BUFFER3 = 0x8828;
    public const uint GL_DRAW_BUFFER4 = 0x8829;
    public const uint GL_DRAW_BUFFER5 = 0x882A;
    public const uint GL_DRAW_BUFFER6 = 0x882B;
    public const uint GL_DRAW_BUFFER7 = 0x882C;
    public const uint GL_DRAW_BUFFER8 = 0x882D;
    public const uint GL_DRAW_BUFFER9 = 0x882E;
    public const uint GL_DRAW_BUFFER10 = 0x882F;
    public const uint GL_DRAW_BUFFER11 = 0x8830;
    public const uint GL_DRAW_BUFFER12 = 0x8831;
    public const uint GL_DRAW_BUFFER13 = 0x8832;
    public const uint GL_DRAW_BUFFER14 = 0x8833;
    public const uint GL_DRAW_BUFFER15 = 0x8834;
    public const uint GL_MAX_FRAGMENT_UNIFORM_COMPONENTS = 0x8B49;
    public const uint GL_MAX_VERTEX_UNIFORM_COMPONENTS = 0x8B4A;
    public const uint GL_SAMPLER_3D = 0x8B5F;
    public const uint GL_SAMPLER_2D_SHADOW = 0x8B62;
    public const uint GL_FRAGMENT_SHADER_DERIVATIVE_HINT = 0x8B8B;
    public const uint GL_PIXEL_PACK_BUFFER = 0x88EB;
    public const uint GL_PIXEL_UNPACK_BUFFER = 0x88EC;
    public const uint GL_PIXEL_PACK_BUFFER_BINDING = 0x88ED;
    public const uint GL_PIXEL_UNPACK_BUFFER_BINDING = 0x88EF;
    public const uint GL_FLOAT_MAT2x3 = 0x8B65;
    public const uint GL_FLOAT_MAT2x4 = 0x8B66;
    public const uint GL_FLOAT_MAT3x2 = 0x8B67;
    public const uint GL_FLOAT_MAT3x4 = 0x8B68;
    public const uint GL_FLOAT_MAT4x2 = 0x8B69;
    public const uint GL_FLOAT_MAT4x3 = 0x8B6A;
    public const uint GL_SRGB = 0x8C40;
    public const uint GL_SRGB8 = 0x8C41;
    public const uint GL_SRGB8_ALPHA8 = 0x8C43;
    public const uint GL_COMPARE_REF_TO_TEXTURE = 0x884E;
    public const uint GL_MAJOR_VERSION = 0x821B;
    public const uint GL_MINOR_VERSION = 0x821C;
    public const uint GL_NUM_EXTENSIONS = 0x821D;
    public const uint GL_RGBA32F = 0x8814;
    public const uint GL_RGB32F = 0x8815;
    public const uint GL_RGBA16F = 0x881A;
    public const uint GL_RGB16F = 0x881B;
    public const uint GL_VERTEX_ATTRIB_ARRAY_INTEGER = 0x88FD;
    public const uint GL_MAX_ARRAY_TEXTURE_LAYERS = 0x88FF;
    public const uint GL_MIN_PROGRAM_TEXEL_OFFSET = 0x8904;
    public const uint GL_MAX_PROGRAM_TEXEL_OFFSET = 0x8905;
    public const uint GL_MAX_VARYING_COMPONENTS = 0x8B4B;
    public const uint GL_TEXTURE_2D_ARRAY = 0x8C1A;
    public const uint GL_TEXTURE_BINDING_2D_ARRAY = 0x8C1D;
    public const uint GL_R11F_G11F_B10F = 0x8C3A;
    public const uint GL_UNSIGNED_INT_10F_11F_11F_REV = 0x8C3B;
    public const uint GL_RGB9_E5 = 0x8C3D;
    public const uint GL_UNSIGNED_INT_5_9_9_9_REV = 0x8C3E;
    public const uint GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH = 0x8C76;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER_MODE = 0x8C7F;
    public const uint GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS = 0x8C80;
    public const uint GL_TRANSFORM_FEEDBACK_VARYINGS = 0x8C83;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER_START = 0x8C84;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER_SIZE = 0x8C85;
    public const uint GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN = 0x8C88;
    public const uint GL_RASTERIZER_DISCARD = 0x8C89;
    public const uint GL_MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS = 0x8C8A;
    public const uint GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS = 0x8C8B;
    public const uint GL_INTERLEAVED_ATTRIBS = 0x8C8C;
    public const uint GL_SEPARATE_ATTRIBS = 0x8C8D;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER = 0x8C8E;
    public const uint GL_TRANSFORM_FEEDBACK_BUFFER_BINDING = 0x8C8F;
    public const uint GL_RGBA32UI = 0x8D70;
    public const uint GL_RGB32UI = 0x8D71;
    public const uint GL_RGBA16UI = 0x8D76;
    public const uint GL_RGB16UI = 0x8D77;
    public const uint GL_RGBA8UI = 0x8D7C;
    public const uint GL_RGB8UI = 0x8D7D;
    public const uint GL_RGBA32I = 0x8D82;
    public const uint GL_RGB32I = 0x8D83;
    public const uint GL_RGBA16I = 0x8D88;
    public const uint GL_RGB16I = 0x8D89;
    public const uint GL_RGBA8I = 0x8D8E;
    public const uint GL_RGB8I = 0x8D8F;
    public const uint GL_RED_INTEGER = 0x8D94;
    public const uint GL_RGB_INTEGER = 0x8D98;
    public const uint GL_RGBA_INTEGER = 0x8D99;
    public const uint GL_SAMPLER_2D_ARRAY = 0x8DC1;
    public const uint GL_SAMPLER_2D_ARRAY_SHADOW = 0x8DC4;
    public const uint GL_SAMPLER_CUBE_SHADOW = 0x8DC5;
    public const uint GL_UNSIGNED_INT_VEC2 = 0x8DC6;
    public const uint GL_UNSIGNED_INT_VEC3 = 0x8DC7;
    public const uint GL_UNSIGNED_INT_VEC4 = 0x8DC8;
    public const uint GL_INT_SAMPLER_2D = 0x8DCA;
    public const uint GL_INT_SAMPLER_3D = 0x8DCB;
    public const uint GL_INT_SAMPLER_CUBE = 0x8DCC;
    public const uint GL_INT_SAMPLER_2D_ARRAY = 0x8DCF;
    public const uint GL_UNSIGNED_INT_SAMPLER_2D = 0x8DD2;
    public const uint GL_UNSIGNED_INT_SAMPLER_3D = 0x8DD3;
    public const uint GL_UNSIGNED_INT_SAMPLER_CUBE = 0x8DD4;
    public const uint GL_UNSIGNED_INT_SAMPLER_2D_ARRAY = 0x8DD7;
    public const uint GL_BUFFER_ACCESS_FLAGS = 0x911F;
    public const uint GL_BUFFER_MAP_LENGTH = 0x9120;
    public const uint GL_BUFFER_MAP_OFFSET = 0x9121;
    public const uint GL_DEPTH_COMPONENT32F = 0x8CAC;
    public const uint GL_DEPTH32F_STENCIL8 = 0x8CAD;
    public const uint GL_FLOAT_32_UNSIGNED_INT_24_8_REV = 0x8DAD;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING = 0x8210;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE = 0x8211;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_RED_SIZE = 0x8212;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_GREEN_SIZE = 0x8213;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_BLUE_SIZE = 0x8214;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE = 0x8215;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE = 0x8216;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE = 0x8217;
    public const uint GL_FRAMEBUFFER_DEFAULT = 0x8218;
    public const uint GL_FRAMEBUFFER_UNDEFINED = 0x8219;
    public const uint GL_DEPTH_STENCIL_ATTACHMENT = 0x821A;
    public const uint GL_DEPTH_STENCIL = 0x84F9;
    public const uint GL_UNSIGNED_INT_24_8 = 0x84FA;
    public const uint GL_DEPTH24_STENCIL8 = 0x88F0;
    public const uint GL_UNSIGNED_NORMALIZED = 0x8C17;
    public const uint GL_DRAW_FRAMEBUFFER_BINDING = 0x8CA6;
    public const uint GL_READ_FRAMEBUFFER = 0x8CA8;
    public const uint GL_DRAW_FRAMEBUFFER = 0x8CA9;
    public const uint GL_READ_FRAMEBUFFER_BINDING = 0x8CAA;
    public const uint GL_RENDERBUFFER_SAMPLES = 0x8CAB;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER = 0x8CD4;
    public const uint GL_MAX_COLOR_ATTACHMENTS = 0x8CDF;
    public const uint GL_COLOR_ATTACHMENT1 = 0x8CE1;
    public const uint GL_COLOR_ATTACHMENT2 = 0x8CE2;
    public const uint GL_COLOR_ATTACHMENT3 = 0x8CE3;
    public const uint GL_COLOR_ATTACHMENT4 = 0x8CE4;
    public const uint GL_COLOR_ATTACHMENT5 = 0x8CE5;
    public const uint GL_COLOR_ATTACHMENT6 = 0x8CE6;
    public const uint GL_COLOR_ATTACHMENT7 = 0x8CE7;
    public const uint GL_COLOR_ATTACHMENT8 = 0x8CE8;
    public const uint GL_COLOR_ATTACHMENT9 = 0x8CE9;
    public const uint GL_COLOR_ATTACHMENT10 = 0x8CEA;
    public const uint GL_COLOR_ATTACHMENT11 = 0x8CEB;
    public const uint GL_COLOR_ATTACHMENT12 = 0x8CEC;
    public const uint GL_COLOR_ATTACHMENT13 = 0x8CED;
    public const uint GL_COLOR_ATTACHMENT14 = 0x8CEE;
    public const uint GL_COLOR_ATTACHMENT15 = 0x8CEF;
    public const uint GL_COLOR_ATTACHMENT16 = 0x8CF0;
    public const uint GL_COLOR_ATTACHMENT17 = 0x8CF1;
    public const uint GL_COLOR_ATTACHMENT18 = 0x8CF2;
    public const uint GL_COLOR_ATTACHMENT19 = 0x8CF3;
    public const uint GL_COLOR_ATTACHMENT20 = 0x8CF4;
    public const uint GL_COLOR_ATTACHMENT21 = 0x8CF5;
    public const uint GL_COLOR_ATTACHMENT22 = 0x8CF6;
    public const uint GL_COLOR_ATTACHMENT23 = 0x8CF7;
    public const uint GL_COLOR_ATTACHMENT24 = 0x8CF8;
    public const uint GL_COLOR_ATTACHMENT25 = 0x8CF9;
    public const uint GL_COLOR_ATTACHMENT26 = 0x8CFA;
    public const uint GL_COLOR_ATTACHMENT27 = 0x8CFB;
    public const uint GL_COLOR_ATTACHMENT28 = 0x8CFC;
    public const uint GL_COLOR_ATTACHMENT29 = 0x8CFD;
    public const uint GL_COLOR_ATTACHMENT30 = 0x8CFE;
    public const uint GL_COLOR_ATTACHMENT31 = 0x8CFF;
    public const uint GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE = 0x8D56;
    public const uint GL_MAX_SAMPLES = 0x8D57;
    public const uint GL_HALF_FLOAT = 0x140B;
    public const uint GL_MAP_READ_BIT = 0x0001;
    public const uint GL_MAP_WRITE_BIT = 0x0002;
    public const uint GL_MAP_INVALIDATE_RANGE_BIT = 0x0004;
    public const uint GL_MAP_INVALIDATE_BUFFER_BIT = 0x0008;
    public const uint GL_MAP_FLUSH_EXPLICIT_BIT = 0x0010;
    public const uint GL_MAP_UNSYNCHRONIZED_BIT = 0x0020;
    public const uint GL_RG = 0x8227;
    public const uint GL_RG_INTEGER = 0x8228;
    public const uint GL_R8 = 0x8229;
    public const uint GL_RG8 = 0x822B;
    public const uint GL_R16F = 0x822D;
    public const uint GL_R32F = 0x822E;
    public const uint GL_RG16F = 0x822F;
    public const uint GL_RG32F = 0x8230;
    public const uint GL_R8I = 0x8231;
    public const uint GL_R8UI = 0x8232;
    public const uint GL_R16I = 0x8233;
    public const uint GL_R16UI = 0x8234;
    public const uint GL_R32I = 0x8235;
    public const uint GL_R32UI = 0x8236;
    public const uint GL_RG8I = 0x8237;
    public const uint GL_RG8UI = 0x8238;
    public const uint GL_RG16I = 0x8239;
    public const uint GL_RG16UI = 0x823A;
    public const uint GL_RG32I = 0x823B;
    public const uint GL_RG32UI = 0x823C;
    public const uint GL_VERTEX_ARRAY_BINDING = 0x85B5;
    public const uint GL_R8_SNORM = 0x8F94;
    public const uint GL_RG8_SNORM = 0x8F95;
    public const uint GL_RGB8_SNORM = 0x8F96;
    public const uint GL_RGBA8_SNORM = 0x8F97;
    public const uint GL_SIGNED_NORMALIZED = 0x8F9C;
    public const uint GL_PRIMITIVE_RESTART_FIXED_INDEX = 0x8D69;
    public const uint GL_COPY_READ_BUFFER = 0x8F36;
    public const uint GL_COPY_WRITE_BUFFER = 0x8F37;
    public const uint GL_COPY_READ_BUFFER_BINDING = 0x8F36;
    public const uint GL_COPY_WRITE_BUFFER_BINDING = 0x8F37;
    public const uint GL_UNIFORM_BUFFER = 0x8A11;
    public const uint GL_UNIFORM_BUFFER_BINDING = 0x8A28;
    public const uint GL_UNIFORM_BUFFER_START = 0x8A29;
    public const uint GL_UNIFORM_BUFFER_SIZE = 0x8A2A;
    public const uint GL_MAX_VERTEX_UNIFORM_BLOCKS = 0x8A2B;
    public const uint GL_MAX_FRAGMENT_UNIFORM_BLOCKS = 0x8A2D;
    public const uint GL_MAX_COMBINED_UNIFORM_BLOCKS = 0x8A2E;
    public const uint GL_MAX_UNIFORM_BUFFER_BINDINGS = 0x8A2F;
    public const uint GL_MAX_UNIFORM_BLOCK_SIZE = 0x8A30;
    public const uint GL_MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS = 0x8A31;
    public const uint GL_MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS = 0x8A33;
    public const uint GL_UNIFORM_BUFFER_OFFSET_ALIGNMENT = 0x8A34;
    public const uint GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH = 0x8A35;
    public const uint GL_ACTIVE_UNIFORM_BLOCKS = 0x8A36;
    public const uint GL_UNIFORM_TYPE = 0x8A37;
    public const uint GL_UNIFORM_SIZE = 0x8A38;
    public const uint GL_UNIFORM_NAME_LENGTH = 0x8A39;
    public const uint GL_UNIFORM_BLOCK_INDEX = 0x8A3A;
    public const uint GL_UNIFORM_OFFSET = 0x8A3B;
    public const uint GL_UNIFORM_ARRAY_STRIDE = 0x8A3C;
    public const uint GL_UNIFORM_MATRIX_STRIDE = 0x8A3D;
    public const uint GL_UNIFORM_IS_ROW_MAJOR = 0x8A3E;
    public const uint GL_UNIFORM_BLOCK_BINDING = 0x8A3F;
    public const uint GL_UNIFORM_BLOCK_DATA_SIZE = 0x8A40;
    public const uint GL_UNIFORM_BLOCK_NAME_LENGTH = 0x8A41;
    public const uint GL_UNIFORM_BLOCK_ACTIVE_UNIFORMS = 0x8A42;
    public const uint GL_UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES = 0x8A43;
    public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER = 0x8A44;
    public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER = 0x8A46;
    public const uint GL_MAX_VERTEX_OUTPUT_COMPONENTS = 0x9122;
    public const uint GL_MAX_FRAGMENT_INPUT_COMPONENTS = 0x9125;
    public const uint GL_MAX_SERVER_WAIT_TIMEOUT = 0x9111;
    public const uint GL_OBJECT_TYPE = 0x9112;
    public const uint GL_SYNC_CONDITION = 0x9113;
    public const uint GL_SYNC_STATUS = 0x9114;
    public const uint GL_SYNC_FLAGS = 0x9115;
    public const uint GL_SYNC_FENCE = 0x9116;
    public const uint GL_SYNC_GPU_COMMANDS_COMPLETE = 0x9117;
    public const uint GL_UNSIGNALED = 0x9118;
    public const uint GL_SIGNALED = 0x9119;
    public const uint GL_ALREADY_SIGNALED = 0x911A;
    public const uint GL_TIMEOUT_EXPIRED = 0x911B;
    public const uint GL_CONDITION_SATISFIED = 0x911C;
    public const uint GL_WAIT_FAILED = 0x911D;
    public const uint GL_SYNC_FLUSH_COMMANDS_BIT = 0x00000001;
    public const uint GL_VERTEX_ATTRIB_ARRAY_DIVISOR = 0x88FE;
    public const uint GL_ANY_SAMPLES_PASSED = 0x8C2F;
    public const uint GL_ANY_SAMPLES_PASSED_CONSERVATIVE = 0x8D6A;
    public const uint GL_SAMPLER_BINDING = 0x8919;
    public const uint GL_RGB10_A2UI = 0x906F;
    public const uint GL_TEXTURE_SWIZZLE_R = 0x8E42;
    public const uint GL_TEXTURE_SWIZZLE_G = 0x8E43;
    public const uint GL_TEXTURE_SWIZZLE_B = 0x8E44;
    public const uint GL_TEXTURE_SWIZZLE_A = 0x8E45;
    public const uint GL_GREEN = 0x1904;
    public const uint GL_BLUE = 0x1905;
    public const uint GL_INT_2_10_10_10_REV = 0x8D9F;
    public const uint GL_TRANSFORM_FEEDBACK = 0x8E22;
    public const uint GL_TRANSFORM_FEEDBACK_PAUSED = 0x8E23;
    public const uint GL_TRANSFORM_FEEDBACK_ACTIVE = 0x8E24;
    public const uint GL_TRANSFORM_FEEDBACK_BINDING = 0x8E25;
    public const uint GL_PROGRAM_BINARY_RETRIEVABLE_HINT = 0x8257;
    public const uint GL_PROGRAM_BINARY_LENGTH = 0x8741;
    public const uint GL_NUM_PROGRAM_BINARY_FORMATS = 0x87FE;
    public const uint GL_PROGRAM_BINARY_FORMATS = 0x87FF;
    public const uint GL_COMPRESSED_R11_EAC = 0x9270;
    public const uint GL_COMPRESSED_SIGNED_R11_EAC = 0x9271;
    public const uint GL_COMPRESSED_RG11_EAC = 0x9272;
    public const uint GL_COMPRESSED_SIGNED_RG11_EAC = 0x9273;
    public const uint GL_COMPRESSED_RGB8_ETC2 = 0x9274;
    public const uint GL_COMPRESSED_SRGB8_ETC2 = 0x9275;
    public const uint GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9276;
    public const uint GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9277;
    public const uint GL_COMPRESSED_RGBA8_ETC2_EAC = 0x9278;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC = 0x9279;
    public const uint GL_TEXTURE_IMMUTABLE_FORMAT = 0x912F;
    public const uint GL_MAX_ELEMENT_INDEX = 0x8D6B;
    public const uint GL_NUM_SAMPLE_COUNTS = 0x9380;
    public const uint GL_TEXTURE_IMMUTABLE_LEVELS = 0x82DF;
    public const uint GL_ES_VERSION_3_1 = 1;
    public const uint GL_COMPUTE_SHADER = 0x91B9;
    public const uint GL_MAX_COMPUTE_UNIFORM_BLOCKS = 0x91BB;
    public const uint GL_MAX_COMPUTE_TEXTURE_IMAGE_UNITS = 0x91BC;
    public const uint GL_MAX_COMPUTE_IMAGE_UNIFORMS = 0x91BD;
    public const uint GL_MAX_COMPUTE_SHARED_MEMORY_SIZE = 0x8262;
    public const uint GL_MAX_COMPUTE_UNIFORM_COMPONENTS = 0x8263;
    public const uint GL_MAX_COMPUTE_ATOMIC_COUNTER_BUFFERS = 0x8264;
    public const uint GL_MAX_COMPUTE_ATOMIC_COUNTERS = 0x8265;
    public const uint GL_MAX_COMBINED_COMPUTE_UNIFORM_COMPONENTS = 0x8266;
    public const uint GL_MAX_COMPUTE_WORK_GROUP_INVOCATIONS = 0x90EB;
    public const uint GL_MAX_COMPUTE_WORK_GROUP_COUNT = 0x91BE;
    public const uint GL_MAX_COMPUTE_WORK_GROUP_SIZE = 0x91BF;
    public const uint GL_COMPUTE_WORK_GROUP_SIZE = 0x8267;
    public const uint GL_DISPATCH_INDIRECT_BUFFER = 0x90EE;
    public const uint GL_DISPATCH_INDIRECT_BUFFER_BINDING = 0x90EF;
    public const uint GL_COMPUTE_SHADER_BIT = 0x00000020;
    public const uint GL_DRAW_INDIRECT_BUFFER = 0x8F3F;
    public const uint GL_DRAW_INDIRECT_BUFFER_BINDING = 0x8F43;
    public const uint GL_MAX_UNIFORM_LOCATIONS = 0x826E;
    public const uint GL_FRAMEBUFFER_DEFAULT_WIDTH = 0x9310;
    public const uint GL_FRAMEBUFFER_DEFAULT_HEIGHT = 0x9311;
    public const uint GL_FRAMEBUFFER_DEFAULT_SAMPLES = 0x9313;
    public const uint GL_FRAMEBUFFER_DEFAULT_FIXED_SAMPLE_LOCATIONS = 0x9314;
    public const uint GL_MAX_FRAMEBUFFER_WIDTH = 0x9315;
    public const uint GL_MAX_FRAMEBUFFER_HEIGHT = 0x9316;
    public const uint GL_MAX_FRAMEBUFFER_SAMPLES = 0x9318;
    public const uint GL_UNIFORM = 0x92E1;
    public const uint GL_UNIFORM_BLOCK = 0x92E2;
    public const uint GL_PROGRAM_INPUT = 0x92E3;
    public const uint GL_PROGRAM_OUTPUT = 0x92E4;
    public const uint GL_BUFFER_VARIABLE = 0x92E5;
    public const uint GL_SHADER_STORAGE_BLOCK = 0x92E6;
    public const uint GL_ATOMIC_COUNTER_BUFFER = 0x92C0;
    public const uint GL_TRANSFORM_FEEDBACK_VARYING = 0x92F4;
    public const uint GL_ACTIVE_RESOURCES = 0x92F5;
    public const uint GL_MAX_NAME_LENGTH = 0x92F6;
    public const uint GL_MAX_NUM_ACTIVE_VARIABLES = 0x92F7;
    public const uint GL_NAME_LENGTH = 0x92F9;
    public const uint GL_TYPE = 0x92FA;
    public const uint GL_ARRAY_SIZE = 0x92FB;
    public const uint GL_OFFSET = 0x92FC;
    public const uint GL_BLOCK_INDEX = 0x92FD;
    public const uint GL_ARRAY_STRIDE = 0x92FE;
    public const uint GL_MATRIX_STRIDE = 0x92FF;
    public const uint GL_IS_ROW_MAJOR = 0x9300;
    public const uint GL_ATOMIC_COUNTER_BUFFER_INDEX = 0x9301;
    public const uint GL_BUFFER_BINDING = 0x9302;
    public const uint GL_BUFFER_DATA_SIZE = 0x9303;
    public const uint GL_NUM_ACTIVE_VARIABLES = 0x9304;
    public const uint GL_ACTIVE_VARIABLES = 0x9305;
    public const uint GL_REFERENCED_BY_VERTEX_SHADER = 0x9306;
    public const uint GL_REFERENCED_BY_FRAGMENT_SHADER = 0x930A;
    public const uint GL_REFERENCED_BY_COMPUTE_SHADER = 0x930B;
    public const uint GL_TOP_LEVEL_ARRAY_SIZE = 0x930C;
    public const uint GL_TOP_LEVEL_ARRAY_STRIDE = 0x930D;
    public const uint GL_LOCATION = 0x930E;
    public const uint GL_VERTEX_SHADER_BIT = 0x00000001;
    public const uint GL_FRAGMENT_SHADER_BIT = 0x00000002;
    public const uint GL_ALL_SHADER_BITS = 0xFFFFFFFF;
    public const uint GL_PROGRAM_SEPARABLE = 0x8258;
    public const uint GL_ACTIVE_PROGRAM = 0x8259;
    public const uint GL_PROGRAM_PIPELINE_BINDING = 0x825A;
    public const uint GL_ATOMIC_COUNTER_BUFFER_BINDING = 0x92C1;
    public const uint GL_ATOMIC_COUNTER_BUFFER_START = 0x92C2;
    public const uint GL_ATOMIC_COUNTER_BUFFER_SIZE = 0x92C3;
    public const uint GL_MAX_VERTEX_ATOMIC_COUNTER_BUFFERS = 0x92CC;
    public const uint GL_MAX_FRAGMENT_ATOMIC_COUNTER_BUFFERS = 0x92D0;
    public const uint GL_MAX_COMBINED_ATOMIC_COUNTER_BUFFERS = 0x92D1;
    public const uint GL_MAX_VERTEX_ATOMIC_COUNTERS = 0x92D2;
    public const uint GL_MAX_FRAGMENT_ATOMIC_COUNTERS = 0x92D6;
    public const uint GL_MAX_COMBINED_ATOMIC_COUNTERS = 0x92D7;
    public const uint GL_MAX_ATOMIC_COUNTER_BUFFER_SIZE = 0x92D8;
    public const uint GL_MAX_ATOMIC_COUNTER_BUFFER_BINDINGS = 0x92DC;
    public const uint GL_ACTIVE_ATOMIC_COUNTER_BUFFERS = 0x92D9;
    public const uint GL_UNSIGNED_INT_ATOMIC_COUNTER = 0x92DB;
    public const uint GL_MAX_IMAGE_UNITS = 0x8F38;
    public const uint GL_MAX_VERTEX_IMAGE_UNIFORMS = 0x90CA;
    public const uint GL_MAX_FRAGMENT_IMAGE_UNIFORMS = 0x90CE;
    public const uint GL_MAX_COMBINED_IMAGE_UNIFORMS = 0x90CF;
    public const uint GL_IMAGE_BINDING_NAME = 0x8F3A;
    public const uint GL_IMAGE_BINDING_LEVEL = 0x8F3B;
    public const uint GL_IMAGE_BINDING_LAYERED = 0x8F3C;
    public const uint GL_IMAGE_BINDING_LAYER = 0x8F3D;
    public const uint GL_IMAGE_BINDING_ACCESS = 0x8F3E;
    public const uint GL_IMAGE_BINDING_FORMAT = 0x906E;
    public const uint GL_VERTEX_ATTRIB_ARRAY_BARRIER_BIT = 0x00000001;
    public const uint GL_ELEMENT_ARRAY_BARRIER_BIT = 0x00000002;
    public const uint GL_UNIFORM_BARRIER_BIT = 0x00000004;
    public const uint GL_TEXTURE_FETCH_BARRIER_BIT = 0x00000008;
    public const uint GL_SHADER_IMAGE_ACCESS_BARRIER_BIT = 0x00000020;
    public const uint GL_COMMAND_BARRIER_BIT = 0x00000040;
    public const uint GL_PIXEL_BUFFER_BARRIER_BIT = 0x00000080;
    public const uint GL_TEXTURE_UPDATE_BARRIER_BIT = 0x00000100;
    public const uint GL_BUFFER_UPDATE_BARRIER_BIT = 0x00000200;
    public const uint GL_FRAMEBUFFER_BARRIER_BIT = 0x00000400;
    public const uint GL_TRANSFORM_FEEDBACK_BARRIER_BIT = 0x00000800;
    public const uint GL_ATOMIC_COUNTER_BARRIER_BIT = 0x00001000;
    public const uint GL_ALL_BARRIER_BITS = 0xFFFFFFFF;
    public const uint GL_IMAGE_2D = 0x904D;
    public const uint GL_IMAGE_3D = 0x904E;
    public const uint GL_IMAGE_CUBE = 0x9050;
    public const uint GL_IMAGE_2D_ARRAY = 0x9053;
    public const uint GL_INT_IMAGE_2D = 0x9058;
    public const uint GL_INT_IMAGE_3D = 0x9059;
    public const uint GL_INT_IMAGE_CUBE = 0x905B;
    public const uint GL_INT_IMAGE_2D_ARRAY = 0x905E;
    public const uint GL_UNSIGNED_INT_IMAGE_2D = 0x9063;
    public const uint GL_UNSIGNED_INT_IMAGE_3D = 0x9064;
    public const uint GL_UNSIGNED_INT_IMAGE_CUBE = 0x9066;
    public const uint GL_UNSIGNED_INT_IMAGE_2D_ARRAY = 0x9069;
    public const uint GL_IMAGE_FORMAT_COMPATIBILITY_TYPE = 0x90C7;
    public const uint GL_IMAGE_FORMAT_COMPATIBILITY_BY_SIZE = 0x90C8;
    public const uint GL_IMAGE_FORMAT_COMPATIBILITY_BY_CLASS = 0x90C9;
    public const uint GL_READ_ONLY = 0x88B8;
    public const uint GL_WRITE_ONLY = 0x88B9;
    public const uint GL_READ_WRITE = 0x88BA;
    public const uint GL_SHADER_STORAGE_BUFFER = 0x90D2;
    public const uint GL_SHADER_STORAGE_BUFFER_BINDING = 0x90D3;
    public const uint GL_SHADER_STORAGE_BUFFER_START = 0x90D4;
    public const uint GL_SHADER_STORAGE_BUFFER_SIZE = 0x90D5;
    public const uint GL_MAX_VERTEX_SHADER_STORAGE_BLOCKS = 0x90D6;
    public const uint GL_MAX_FRAGMENT_SHADER_STORAGE_BLOCKS = 0x90DA;
    public const uint GL_MAX_COMPUTE_SHADER_STORAGE_BLOCKS = 0x90DB;
    public const uint GL_MAX_COMBINED_SHADER_STORAGE_BLOCKS = 0x90DC;
    public const uint GL_MAX_SHADER_STORAGE_BUFFER_BINDINGS = 0x90DD;
    public const uint GL_MAX_SHADER_STORAGE_BLOCK_SIZE = 0x90DE;
    public const uint GL_SHADER_STORAGE_BUFFER_OFFSET_ALIGNMENT = 0x90DF;
    public const uint GL_SHADER_STORAGE_BARRIER_BIT = 0x00002000;
    public const uint GL_MAX_COMBINED_SHADER_OUTPUT_RESOURCES = 0x8F39;
    public const uint GL_DEPTH_STENCIL_TEXTURE_MODE = 0x90EA;
    public const uint GL_STENCIL_INDEX = 0x1901;
    public const uint GL_MIN_PROGRAM_TEXTURE_GATHER_OFFSET = 0x8E5E;
    public const uint GL_MAX_PROGRAM_TEXTURE_GATHER_OFFSET = 0x8E5F;
    public const uint GL_SAMPLE_POSITION = 0x8E50;
    public const uint GL_SAMPLE_MASK = 0x8E51;
    public const uint GL_SAMPLE_MASK_VALUE = 0x8E52;
    public const uint GL_TEXTURE_2D_MULTISAMPLE = 0x9100;
    public const uint GL_MAX_SAMPLE_MASK_WORDS = 0x8E59;
    public const uint GL_MAX_COLOR_TEXTURE_SAMPLES = 0x910E;
    public const uint GL_MAX_DEPTH_TEXTURE_SAMPLES = 0x910F;
    public const uint GL_MAX_INTEGER_SAMPLES = 0x9110;
    public const uint GL_TEXTURE_BINDING_2D_MULTISAMPLE = 0x9104;
    public const uint GL_TEXTURE_SAMPLES = 0x9106;
    public const uint GL_TEXTURE_FIXED_SAMPLE_LOCATIONS = 0x9107;
    public const uint GL_TEXTURE_WIDTH = 0x1000;
    public const uint GL_TEXTURE_HEIGHT = 0x1001;
    public const uint GL_TEXTURE_DEPTH = 0x8071;
    public const uint GL_TEXTURE_INTERNAL_FORMAT = 0x1003;
    public const uint GL_TEXTURE_RED_SIZE = 0x805C;
    public const uint GL_TEXTURE_GREEN_SIZE = 0x805D;
    public const uint GL_TEXTURE_BLUE_SIZE = 0x805E;
    public const uint GL_TEXTURE_ALPHA_SIZE = 0x805F;
    public const uint GL_TEXTURE_DEPTH_SIZE = 0x884A;
    public const uint GL_TEXTURE_STENCIL_SIZE = 0x88F1;
    public const uint GL_TEXTURE_SHARED_SIZE = 0x8C3F;
    public const uint GL_TEXTURE_RED_TYPE = 0x8C10;
    public const uint GL_TEXTURE_GREEN_TYPE = 0x8C11;
    public const uint GL_TEXTURE_BLUE_TYPE = 0x8C12;
    public const uint GL_TEXTURE_ALPHA_TYPE = 0x8C13;
    public const uint GL_TEXTURE_DEPTH_TYPE = 0x8C16;
    public const uint GL_TEXTURE_COMPRESSED = 0x86A1;
    public const uint GL_SAMPLER_2D_MULTISAMPLE = 0x9108;
    public const uint GL_INT_SAMPLER_2D_MULTISAMPLE = 0x9109;
    public const uint GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE = 0x910A;
    public const uint GL_VERTEX_ATTRIB_BINDING = 0x82D4;
    public const uint GL_VERTEX_ATTRIB_RELATIVE_OFFSET = 0x82D5;
    public const uint GL_VERTEX_BINDING_DIVISOR = 0x82D6;
    public const uint GL_VERTEX_BINDING_OFFSET = 0x82D7;
    public const uint GL_VERTEX_BINDING_STRIDE = 0x82D8;
    public const uint GL_VERTEX_BINDING_BUFFER = 0x8F4F;
    public const uint GL_MAX_VERTEX_ATTRIB_RELATIVE_OFFSET = 0x82D9;
    public const uint GL_MAX_VERTEX_ATTRIB_BINDINGS = 0x82DA;
    public const uint GL_MAX_VERTEX_ATTRIB_STRIDE = 0x82E5;
    public const uint GL_ES_VERSION_3_2 = 1;
    public const uint GL_MULTISAMPLE_LINE_WIDTH_RANGE = 0x9381;
    public const uint GL_MULTISAMPLE_LINE_WIDTH_GRANULARITY = 0x9382;
    public const uint GL_MULTIPLY = 0x9294;
    public const uint GL_SCREEN = 0x9295;
    public const uint GL_OVERLAY = 0x9296;
    public const uint GL_DARKEN = 0x9297;
    public const uint GL_LIGHTEN = 0x9298;
    public const uint GL_COLORDODGE = 0x9299;
    public const uint GL_COLORBURN = 0x929A;
    public const uint GL_HARDLIGHT = 0x929B;
    public const uint GL_SOFTLIGHT = 0x929C;
    public const uint GL_DIFFERENCE = 0x929E;
    public const uint GL_EXCLUSION = 0x92A0;
    public const uint GL_HSL_HUE = 0x92AD;
    public const uint GL_HSL_SATURATION = 0x92AE;
    public const uint GL_HSL_COLOR = 0x92AF;
    public const uint GL_HSL_LUMINOSITY = 0x92B0;
    public const uint GL_DEBUG_OUTPUT_SYNCHRONOUS = 0x8242;
    public const uint GL_DEBUG_NEXT_LOGGED_MESSAGE_LENGTH = 0x8243;
    public const uint GL_DEBUG_CALLBACK_FUNCTION = 0x8244;
    public const uint GL_DEBUG_CALLBACK_USER_PARAM = 0x8245;
    public const uint GL_DEBUG_SOURCE_API = 0x8246;
    public const uint GL_DEBUG_SOURCE_WINDOW_SYSTEM = 0x8247;
    public const uint GL_DEBUG_SOURCE_SHADER_COMPILER = 0x8248;
    public const uint GL_DEBUG_SOURCE_THIRD_PARTY = 0x8249;
    public const uint GL_DEBUG_SOURCE_APPLICATION = 0x824A;
    public const uint GL_DEBUG_SOURCE_OTHER = 0x824B;
    public const uint GL_DEBUG_TYPE_ERROR = 0x824C;
    public const uint GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR = 0x824D;
    public const uint GL_DEBUG_TYPE_UNDEFINED_BEHAVIOR = 0x824E;
    public const uint GL_DEBUG_TYPE_PORTABILITY = 0x824F;
    public const uint GL_DEBUG_TYPE_PERFORMANCE = 0x8250;
    public const uint GL_DEBUG_TYPE_OTHER = 0x8251;
    public const uint GL_DEBUG_TYPE_MARKER = 0x8268;
    public const uint GL_DEBUG_TYPE_PUSH_GROUP = 0x8269;
    public const uint GL_DEBUG_TYPE_POP_GROUP = 0x826A;
    public const uint GL_DEBUG_SEVERITY_NOTIFICATION = 0x826B;
    public const uint GL_MAX_DEBUG_GROUP_STACK_DEPTH = 0x826C;
    public const uint GL_DEBUG_GROUP_STACK_DEPTH = 0x826D;
    public const uint GL_BUFFER = 0x82E0;
    public const uint GL_SHADER = 0x82E1;
    public const uint GL_PROGRAM = 0x82E2;
    public const uint GL_VERTEX_ARRAY = 0x8074;
    public const uint GL_QUERY = 0x82E3;
    public const uint GL_PROGRAM_PIPELINE = 0x82E4;
    public const uint GL_SAMPLER = 0x82E6;
    public const uint GL_MAX_LABEL_LENGTH = 0x82E8;
    public const uint GL_MAX_DEBUG_MESSAGE_LENGTH = 0x9143;
    public const uint GL_MAX_DEBUG_LOGGED_MESSAGES = 0x9144;
    public const uint GL_DEBUG_LOGGED_MESSAGES = 0x9145;
    public const uint GL_DEBUG_SEVERITY_HIGH = 0x9146;
    public const uint GL_DEBUG_SEVERITY_MEDIUM = 0x9147;
    public const uint GL_DEBUG_SEVERITY_LOW = 0x9148;
    public const uint GL_DEBUG_OUTPUT = 0x92E0;
    public const uint GL_CONTEXT_FLAG_DEBUG_BIT = 0x00000002;
    public const uint GL_STACK_OVERFLOW = 0x0503;
    public const uint GL_STACK_UNDERFLOW = 0x0504;
    public const uint GL_GEOMETRY_SHADER = 0x8DD9;
    public const uint GL_GEOMETRY_SHADER_BIT = 0x00000004;
    public const uint GL_GEOMETRY_VERTICES_OUT = 0x8916;
    public const uint GL_GEOMETRY_INPUT_TYPE = 0x8917;
    public const uint GL_GEOMETRY_OUTPUT_TYPE = 0x8918;
    public const uint GL_GEOMETRY_SHADER_INVOCATIONS = 0x887F;
    public const uint GL_LAYER_PROVOKING_VERTEX = 0x825E;
    public const uint GL_LINES_ADJACENCY = 0x000A;
    public const uint GL_LINE_STRIP_ADJACENCY = 0x000B;
    public const uint GL_TRIANGLES_ADJACENCY = 0x000C;
    public const uint GL_TRIANGLE_STRIP_ADJACENCY = 0x000D;
    public const uint GL_MAX_GEOMETRY_UNIFORM_COMPONENTS = 0x8DDF;
    public const uint GL_MAX_GEOMETRY_UNIFORM_BLOCKS = 0x8A2C;
    public const uint GL_MAX_COMBINED_GEOMETRY_UNIFORM_COMPONENTS = 0x8A32;
    public const uint GL_MAX_GEOMETRY_INPUT_COMPONENTS = 0x9123;
    public const uint GL_MAX_GEOMETRY_OUTPUT_COMPONENTS = 0x9124;
    public const uint GL_MAX_GEOMETRY_OUTPUT_VERTICES = 0x8DE0;
    public const uint GL_MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS = 0x8DE1;
    public const uint GL_MAX_GEOMETRY_SHADER_INVOCATIONS = 0x8E5A;
    public const uint GL_MAX_GEOMETRY_TEXTURE_IMAGE_UNITS = 0x8C29;
    public const uint GL_MAX_GEOMETRY_ATOMIC_COUNTER_BUFFERS = 0x92CF;
    public const uint GL_MAX_GEOMETRY_ATOMIC_COUNTERS = 0x92D5;
    public const uint GL_MAX_GEOMETRY_IMAGE_UNIFORMS = 0x90CD;
    public const uint GL_MAX_GEOMETRY_SHADER_STORAGE_BLOCKS = 0x90D7;
    public const uint GL_FIRST_VERTEX_CONVENTION = 0x8E4D;
    public const uint GL_LAST_VERTEX_CONVENTION = 0x8E4E;
    public const uint GL_UNDEFINED_VERTEX = 0x8260;
    public const uint GL_PRIMITIVES_GENERATED = 0x8C87;
    public const uint GL_FRAMEBUFFER_DEFAULT_LAYERS = 0x9312;
    public const uint GL_MAX_FRAMEBUFFER_LAYERS = 0x9317;
    public const uint GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS = 0x8DA8;
    public const uint GL_FRAMEBUFFER_ATTACHMENT_LAYERED = 0x8DA7;
    public const uint GL_REFERENCED_BY_GEOMETRY_SHADER = 0x9309;
    public const uint GL_PRIMITIVE_BOUNDING_BOX = 0x92BE;
    public const uint GL_CONTEXT_FLAG_ROBUST_ACCESS_BIT = 0x00000004;
    public const uint GL_CONTEXT_FLAGS = 0x821E;
    public const uint GL_LOSE_CONTEXT_ON_RESET = 0x8252;
    public const uint GL_GUILTY_CONTEXT_RESET = 0x8253;
    public const uint GL_INNOCENT_CONTEXT_RESET = 0x8254;
    public const uint GL_UNKNOWN_CONTEXT_RESET = 0x8255;
    public const uint GL_RESET_NOTIFICATION_STRATEGY = 0x8256;
    public const uint GL_NO_RESET_NOTIFICATION = 0x8261;
    public const uint GL_CONTEXT_LOST = 0x0507;
    public const uint GL_SAMPLE_SHADING = 0x8C36;
    public const uint GL_MIN_SAMPLE_SHADING_VALUE = 0x8C37;
    public const uint GL_MIN_FRAGMENT_INTERPOLATION_OFFSET = 0x8E5B;
    public const uint GL_MAX_FRAGMENT_INTERPOLATION_OFFSET = 0x8E5C;
    public const uint GL_FRAGMENT_INTERPOLATION_OFFSET_BITS = 0x8E5D;
    public const uint GL_PATCHES = 0x000E;
    public const uint GL_PATCH_VERTICES = 0x8E72;
    public const uint GL_TESS_CONTROL_OUTPUT_VERTICES = 0x8E75;
    public const uint GL_TESS_GEN_MODE = 0x8E76;
    public const uint GL_TESS_GEN_SPACING = 0x8E77;
    public const uint GL_TESS_GEN_VERTEX_ORDER = 0x8E78;
    public const uint GL_TESS_GEN_POINT_MODE = 0x8E79;
    public const uint GL_ISOLINES = 0x8E7A;
    public const uint GL_QUADS = 0x0007;
    public const uint GL_FRACTIONAL_ODD = 0x8E7B;
    public const uint GL_FRACTIONAL_EVEN = 0x8E7C;
    public const uint GL_MAX_PATCH_VERTICES = 0x8E7D;
    public const uint GL_MAX_TESS_GEN_LEVEL = 0x8E7E;
    public const uint GL_MAX_TESS_CONTROL_UNIFORM_COMPONENTS = 0x8E7F;
    public const uint GL_MAX_TESS_EVALUATION_UNIFORM_COMPONENTS = 0x8E80;
    public const uint GL_MAX_TESS_CONTROL_TEXTURE_IMAGE_UNITS = 0x8E81;
    public const uint GL_MAX_TESS_EVALUATION_TEXTURE_IMAGE_UNITS = 0x8E82;
    public const uint GL_MAX_TESS_CONTROL_OUTPUT_COMPONENTS = 0x8E83;
    public const uint GL_MAX_TESS_PATCH_COMPONENTS = 0x8E84;
    public const uint GL_MAX_TESS_CONTROL_TOTAL_OUTPUT_COMPONENTS = 0x8E85;
    public const uint GL_MAX_TESS_EVALUATION_OUTPUT_COMPONENTS = 0x8E86;
    public const uint GL_MAX_TESS_CONTROL_UNIFORM_BLOCKS = 0x8E89;
    public const uint GL_MAX_TESS_EVALUATION_UNIFORM_BLOCKS = 0x8E8A;
    public const uint GL_MAX_TESS_CONTROL_INPUT_COMPONENTS = 0x886C;
    public const uint GL_MAX_TESS_EVALUATION_INPUT_COMPONENTS = 0x886D;
    public const uint GL_MAX_COMBINED_TESS_CONTROL_UNIFORM_COMPONENTS = 0x8E1E;
    public const uint GL_MAX_COMBINED_TESS_EVALUATION_UNIFORM_COMPONENTS = 0x8E1F;
    public const uint GL_MAX_TESS_CONTROL_ATOMIC_COUNTER_BUFFERS = 0x92CD;
    public const uint GL_MAX_TESS_EVALUATION_ATOMIC_COUNTER_BUFFERS = 0x92CE;
    public const uint GL_MAX_TESS_CONTROL_ATOMIC_COUNTERS = 0x92D3;
    public const uint GL_MAX_TESS_EVALUATION_ATOMIC_COUNTERS = 0x92D4;
    public const uint GL_MAX_TESS_CONTROL_IMAGE_UNIFORMS = 0x90CB;
    public const uint GL_MAX_TESS_EVALUATION_IMAGE_UNIFORMS = 0x90CC;
    public const uint GL_MAX_TESS_CONTROL_SHADER_STORAGE_BLOCKS = 0x90D8;
    public const uint GL_MAX_TESS_EVALUATION_SHADER_STORAGE_BLOCKS = 0x90D9;
    public const uint GL_PRIMITIVE_RESTART_FOR_PATCHES_SUPPORTED = 0x8221;
    public const uint GL_IS_PER_PATCH = 0x92E7;
    public const uint GL_REFERENCED_BY_TESS_CONTROL_SHADER = 0x9307;
    public const uint GL_REFERENCED_BY_TESS_EVALUATION_SHADER = 0x9308;
    public const uint GL_TESS_CONTROL_SHADER = 0x8E88;
    public const uint GL_TESS_EVALUATION_SHADER = 0x8E87;
    public const uint GL_TESS_CONTROL_SHADER_BIT = 0x00000008;
    public const uint GL_TESS_EVALUATION_SHADER_BIT = 0x00000010;
    public const uint GL_TEXTURE_BORDER_COLOR = 0x1004;
    public const uint GL_CLAMP_TO_BORDER = 0x812D;
    public const uint GL_TEXTURE_BUFFER = 0x8C2A;
    public const uint GL_TEXTURE_BUFFER_BINDING = 0x8C2A;
    public const uint GL_MAX_TEXTURE_BUFFER_SIZE = 0x8C2B;
    public const uint GL_TEXTURE_BINDING_BUFFER = 0x8C2C;
    public const uint GL_TEXTURE_BUFFER_DATA_STORE_BINDING = 0x8C2D;
    public const uint GL_TEXTURE_BUFFER_OFFSET_ALIGNMENT = 0x919F;
    public const uint GL_SAMPLER_BUFFER = 0x8DC2;
    public const uint GL_INT_SAMPLER_BUFFER = 0x8DD0;
    public const uint GL_UNSIGNED_INT_SAMPLER_BUFFER = 0x8DD8;
    public const uint GL_IMAGE_BUFFER = 0x9051;
    public const uint GL_INT_IMAGE_BUFFER = 0x905C;
    public const uint GL_UNSIGNED_INT_IMAGE_BUFFER = 0x9067;
    public const uint GL_TEXTURE_BUFFER_OFFSET = 0x919D;
    public const uint GL_TEXTURE_BUFFER_SIZE = 0x919E;
    public const uint GL_COMPRESSED_RGBA_ASTC_4x4 = 0x93B0;
    public const uint GL_COMPRESSED_RGBA_ASTC_5x4 = 0x93B1;
    public const uint GL_COMPRESSED_RGBA_ASTC_5x5 = 0x93B2;
    public const uint GL_COMPRESSED_RGBA_ASTC_6x5 = 0x93B3;
    public const uint GL_COMPRESSED_RGBA_ASTC_6x6 = 0x93B4;
    public const uint GL_COMPRESSED_RGBA_ASTC_8x5 = 0x93B5;
    public const uint GL_COMPRESSED_RGBA_ASTC_8x6 = 0x93B6;
    public const uint GL_COMPRESSED_RGBA_ASTC_8x8 = 0x93B7;
    public const uint GL_COMPRESSED_RGBA_ASTC_10x5 = 0x93B8;
    public const uint GL_COMPRESSED_RGBA_ASTC_10x6 = 0x93B9;
    public const uint GL_COMPRESSED_RGBA_ASTC_10x8 = 0x93BA;
    public const uint GL_COMPRESSED_RGBA_ASTC_10x10 = 0x93BB;
    public const uint GL_COMPRESSED_RGBA_ASTC_12x10 = 0x93BC;
    public const uint GL_COMPRESSED_RGBA_ASTC_12x12 = 0x93BD;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_4x4 = 0x93D0;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x4 = 0x93D1;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_5x5 = 0x93D2;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x5 = 0x93D3;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_6x6 = 0x93D4;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x5 = 0x93D5;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x6 = 0x93D6;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_8x8 = 0x93D7;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x5 = 0x93D8;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x6 = 0x93D9;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x8 = 0x93DA;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_10x10 = 0x93DB;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x10 = 0x93DC;
    public const uint GL_COMPRESSED_SRGB8_ALPHA8_ASTC_12x12 = 0x93DD;
    public const uint GL_TEXTURE_CUBE_MAP_ARRAY = 0x9009;
    public const uint GL_TEXTURE_BINDING_CUBE_MAP_ARRAY = 0x900A;
    public const uint GL_SAMPLER_CUBE_MAP_ARRAY = 0x900C;
    public const uint GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW = 0x900D;
    public const uint GL_INT_SAMPLER_CUBE_MAP_ARRAY = 0x900E;
    public const uint GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY = 0x900F;
    public const uint GL_IMAGE_CUBE_MAP_ARRAY = 0x9054;
    public const uint GL_INT_IMAGE_CUBE_MAP_ARRAY = 0x905F;
    public const uint GL_UNSIGNED_INT_IMAGE_CUBE_MAP_ARRAY = 0x906A;
    public const uint GL_TEXTURE_2D_MULTISAMPLE_ARRAY = 0x9102;
    public const uint GL_TEXTURE_BINDING_2D_MULTISAMPLE_ARRAY = 0x9105;
    public const uint GL_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910B;
    public const uint GL_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910C;
    public const uint GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910D;
    private static nint pfn_glActiveTexture;
    public static void glActiveTexture(uint texture) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glActiveTexture)(texture);
    
    private static nint pfn_glAttachShader;
    public static void glAttachShader(uint program, uint shader) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glAttachShader)(program, shader);
    
    private static nint pfn_glBindAttribLocation;
    public static void glBindAttribLocation(uint program, uint index, byte* name) => ((delegate* unmanaged[Stdcall]<uint, uint, byte*, void>)pfn_glBindAttribLocation)(program, index, name);
    
    private static nint pfn_glBindBuffer;
    public static void glBindBuffer(uint target, uint buffer) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glBindBuffer)(target, buffer);
    
    private static nint pfn_glBindFramebuffer;
    public static void glBindFramebuffer(uint target, uint framebuffer) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glBindFramebuffer)(target, framebuffer);
    
    private static nint pfn_glBindRenderbuffer;
    public static void glBindRenderbuffer(uint target, uint renderbuffer) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glBindRenderbuffer)(target, renderbuffer);
    
    private static nint pfn_glBindTexture;
    public static void glBindTexture(uint target, uint texture) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glBindTexture)(target, texture);
    
    private static nint pfn_glBlendColor;
    public static void glBlendColor(float red, float green, float blue, float alpha) => ((delegate* unmanaged[Stdcall]<float, float, float, float, void>)pfn_glBlendColor)(red, green, blue, alpha);
    
    private static nint pfn_glBlendEquation;
    public static void glBlendEquation(uint mode) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glBlendEquation)(mode);
    
    private static nint pfn_glBlendEquationSeparate;
    public static void glBlendEquationSeparate(uint modeRGB, uint modeAlpha) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glBlendEquationSeparate)(modeRGB, modeAlpha);
    
    private static nint pfn_glBlendFunc;
    public static void glBlendFunc(uint sfactor, uint dfactor) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glBlendFunc)(sfactor, dfactor);
    
    private static nint pfn_glBlendFuncSeparate;
    public static void glBlendFuncSeparate(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void>)pfn_glBlendFuncSeparate)(sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha);
    
    private static nint pfn_glBufferData;
    public static void glBufferData(uint target, int size, void* data, uint usage) => ((delegate* unmanaged[Stdcall]<uint, int, void*, uint, void>)pfn_glBufferData)(target, size, data, usage);
    
    private static nint pfn_glBufferSubData;
    public static void glBufferSubData(uint target, int offset, int size, void* data) => ((delegate* unmanaged[Stdcall]<uint, int, int, void*, void>)pfn_glBufferSubData)(target, offset, size, data);
    
    private static nint pfn_glCheckFramebufferStatus;
    public static uint glCheckFramebufferStatus(uint target) => ((delegate* unmanaged[Stdcall]<uint, uint>)pfn_glCheckFramebufferStatus)(target);
    
    private static nint pfn_glClear;
    public static void glClear(uint mask) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glClear)(mask);
    
    private static nint pfn_glClearColor;
    public static void glClearColor(float red, float green, float blue, float alpha) => ((delegate* unmanaged[Stdcall]<float, float, float, float, void>)pfn_glClearColor)(red, green, blue, alpha);
    
    private static nint pfn_glClearDepthf;
    public static void glClearDepthf(float d) => ((delegate* unmanaged[Stdcall]<float, void>)pfn_glClearDepthf)(d);
    
    private static nint pfn_glClearStencil;
    public static void glClearStencil(int s) => ((delegate* unmanaged[Stdcall]<int, void>)pfn_glClearStencil)(s);
    
    private static nint pfn_glColorMask;
    public static void glColorMask(byte red, byte green, byte blue, byte alpha) => ((delegate* unmanaged[Stdcall]<byte, byte, byte, byte, void>)pfn_glColorMask)(red, green, blue, alpha);
    
    private static nint pfn_glCompileShader;
    public static void glCompileShader(uint shader) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glCompileShader)(shader);
    
    private static nint pfn_glCompressedTexImage2D;
    public static void glCompressedTexImage2D(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, void* data) => ((delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, int, void*, void>)pfn_glCompressedTexImage2D)(target, level, internalformat, width, height, border, imageSize, data);
    
    private static nint pfn_glCompressedTexSubImage2D;
    public static void glCompressedTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, void* data) => ((delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, uint, int, void*, void>)pfn_glCompressedTexSubImage2D)(target, level, xoffset, yoffset, width, height, format, imageSize, data);
    
    private static nint pfn_glCopyTexImage2D;
    public static void glCopyTexImage2D(uint target, int level, uint internalformat, int x, int y, int width, int height, int border) => ((delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, int, int, void>)pfn_glCopyTexImage2D)(target, level, internalformat, x, y, width, height, border);
    
    private static nint pfn_glCopyTexSubImage2D;
    public static void glCopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height) => ((delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, void>)pfn_glCopyTexSubImage2D)(target, level, xoffset, yoffset, x, y, width, height);
    
    private static nint pfn_glCreateProgram;
    public static uint glCreateProgram() => ((delegate* unmanaged[Stdcall]<uint>)pfn_glCreateProgram)();
    
    private static nint pfn_glCreateShader;
    public static uint glCreateShader(uint type) => ((delegate* unmanaged[Stdcall]<uint, uint>)pfn_glCreateShader)(type);
    
    private static nint pfn_glCullFace;
    public static void glCullFace(uint mode) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glCullFace)(mode);
    
    private static nint pfn_glDeleteBuffers;
    public static void glDeleteBuffers(int n, uint* buffers) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glDeleteBuffers)(n, buffers);
    
    private static nint pfn_glDeleteFramebuffers;
    public static void glDeleteFramebuffers(int n, uint* framebuffers) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glDeleteFramebuffers)(n, framebuffers);
    
    private static nint pfn_glDeleteProgram;
    public static void glDeleteProgram(uint program) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glDeleteProgram)(program);
    
    private static nint pfn_glDeleteRenderbuffers;
    public static void glDeleteRenderbuffers(int n, uint* renderbuffers) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glDeleteRenderbuffers)(n, renderbuffers);
    
    private static nint pfn_glDeleteShader;
    public static void glDeleteShader(uint shader) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glDeleteShader)(shader);
    
    private static nint pfn_glDeleteTextures;
    public static void glDeleteTextures(int n, uint* textures) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glDeleteTextures)(n, textures);
    
    private static nint pfn_glDepthFunc;
    public static void glDepthFunc(uint func) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glDepthFunc)(func);
    
    private static nint pfn_glDepthMask;
    public static void glDepthMask(byte flag) => ((delegate* unmanaged[Stdcall]<byte, void>)pfn_glDepthMask)(flag);
    
    private static nint pfn_glDepthRangef;
    public static void glDepthRangef(float n, float f) => ((delegate* unmanaged[Stdcall]<float, float, void>)pfn_glDepthRangef)(n, f);
    
    private static nint pfn_glDetachShader;
    public static void glDetachShader(uint program, uint shader) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glDetachShader)(program, shader);
    
    private static nint pfn_glDisable;
    public static void glDisable(uint cap) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glDisable)(cap);
    
    private static nint pfn_glDisableVertexAttribArray;
    public static void glDisableVertexAttribArray(uint index) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glDisableVertexAttribArray)(index);
    
    private static nint pfn_glDrawArrays;
    public static void glDrawArrays(uint mode, int first, int count) => ((delegate* unmanaged[Stdcall]<uint, int, int, void>)pfn_glDrawArrays)(mode, first, count);
    
    private static nint pfn_glDrawElements;
    public static void glDrawElements(uint mode, int count, uint type, void* indices) => ((delegate* unmanaged[Stdcall]<uint, int, uint, void*, void>)pfn_glDrawElements)(mode, count, type, indices);
    
    private static nint pfn_glEnable;
    public static void glEnable(uint cap) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glEnable)(cap);
    
    private static nint pfn_glEnableVertexAttribArray;
    public static void glEnableVertexAttribArray(uint index) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glEnableVertexAttribArray)(index);
    
    private static nint pfn_glFinish;
    public static void glFinish() => ((delegate* unmanaged[Stdcall]<void>)pfn_glFinish)();
    
    private static nint pfn_glFlush;
    public static void glFlush() => ((delegate* unmanaged[Stdcall]<void>)pfn_glFlush)();
    
    private static nint pfn_glFramebufferRenderbuffer;
    public static void glFramebufferRenderbuffer(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void>)pfn_glFramebufferRenderbuffer)(target, attachment, renderbuffertarget, renderbuffer);
    
    private static nint pfn_glFramebufferTexture2D;
    public static void glFramebufferTexture2D(uint target, uint attachment, uint textarget, uint texture, int level) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, uint, int, void>)pfn_glFramebufferTexture2D)(target, attachment, textarget, texture, level);
    
    private static nint pfn_glFrontFace;
    public static void glFrontFace(uint mode) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glFrontFace)(mode);
    
    private static nint pfn_glGenBuffers;
    public static void glGenBuffers(int n, uint* buffers) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glGenBuffers)(n, buffers);
    
    private static nint pfn_glGenerateMipmap;
    public static void glGenerateMipmap(uint target) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glGenerateMipmap)(target);
    
    private static nint pfn_glGenFramebuffers;
    public static void glGenFramebuffers(int n, uint* framebuffers) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glGenFramebuffers)(n, framebuffers);
    
    private static nint pfn_glGenRenderbuffers;
    public static void glGenRenderbuffers(int n, uint* renderbuffers) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glGenRenderbuffers)(n, renderbuffers);
    
    private static nint pfn_glGenTextures;
    public static void glGenTextures(int n, uint* textures) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glGenTextures)(n, textures);
    
    private static nint pfn_glGetActiveAttrib;
    public static void glGetActiveAttrib(uint program, uint index, int bufSize, int* length, int* size, uint* type, byte* name) => ((delegate* unmanaged[Stdcall]<uint, uint, int, int*, int*, uint*, byte*, void>)pfn_glGetActiveAttrib)(program, index, bufSize, length, size, type, name);
    
    private static nint pfn_glGetActiveUniform;
    public static void glGetActiveUniform(uint program, uint index, int bufSize, int* length, int* size, uint* type, byte* name) => ((delegate* unmanaged[Stdcall]<uint, uint, int, int*, int*, uint*, byte*, void>)pfn_glGetActiveUniform)(program, index, bufSize, length, size, type, name);
    
    private static nint pfn_glGetAttachedShaders;
    public static void glGetAttachedShaders(uint program, int maxCount, int* count, uint* shaders) => ((delegate* unmanaged[Stdcall]<uint, int, int*, uint*, void>)pfn_glGetAttachedShaders)(program, maxCount, count, shaders);
    
    private static nint pfn_glGetAttribLocation;
    public static int glGetAttribLocation(uint program, byte* name) => ((delegate* unmanaged[Stdcall]<uint, byte*, int>)pfn_glGetAttribLocation)(program, name);
    
    private static nint pfn_glGetBooleanv;
    public static void glGetBooleanv(uint pname, byte* data) => ((delegate* unmanaged[Stdcall]<uint, byte*, void>)pfn_glGetBooleanv)(pname, data);
    
    private static nint pfn_glGetBufferParameteriv;
    public static void glGetBufferParameteriv(uint target, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetBufferParameteriv)(target, pname, @params);
    
    private static nint pfn_glGetError;
    public static uint glGetError() => ((delegate* unmanaged[Stdcall]<uint>)pfn_glGetError)();
    
    private static nint pfn_glGetFloatv;
    public static void glGetFloatv(uint pname, float* data) => ((delegate* unmanaged[Stdcall]<uint, float*, void>)pfn_glGetFloatv)(pname, data);
    
    private static nint pfn_glGetFramebufferAttachmentParameteriv;
    public static void glGetFramebufferAttachmentParameteriv(uint target, uint attachment, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void>)pfn_glGetFramebufferAttachmentParameteriv)(target, attachment, pname, @params);
    
    private static nint pfn_glGetIntegerv;
    public static void glGetIntegerv(uint pname, int* data) => ((delegate* unmanaged[Stdcall]<uint, int*, void>)pfn_glGetIntegerv)(pname, data);
    
    private static nint pfn_glGetProgramiv;
    public static void glGetProgramiv(uint program, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetProgramiv)(program, pname, @params);
    
    private static nint pfn_glGetProgramInfoLog;
    public static void glGetProgramInfoLog(uint program, int bufSize, int* length, byte* infoLog) => ((delegate* unmanaged[Stdcall]<uint, int, int*, byte*, void>)pfn_glGetProgramInfoLog)(program, bufSize, length, infoLog);
    
    private static nint pfn_glGetRenderbufferParameteriv;
    public static void glGetRenderbufferParameteriv(uint target, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetRenderbufferParameteriv)(target, pname, @params);
    
    private static nint pfn_glGetShaderiv;
    public static void glGetShaderiv(uint shader, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetShaderiv)(shader, pname, @params);
    
    private static nint pfn_glGetShaderInfoLog;
    public static void glGetShaderInfoLog(uint shader, int bufSize, int* length, byte* infoLog) => ((delegate* unmanaged[Stdcall]<uint, int, int*, byte*, void>)pfn_glGetShaderInfoLog)(shader, bufSize, length, infoLog);
    
    private static nint pfn_glGetShaderPrecisionFormat;
    public static void glGetShaderPrecisionFormat(uint shadertype, uint precisiontype, int* range, int* precision) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, int*, void>)pfn_glGetShaderPrecisionFormat)(shadertype, precisiontype, range, precision);
    
    private static nint pfn_glGetShaderSource;
    public static void glGetShaderSource(uint shader, int bufSize, int* length, byte* source) => ((delegate* unmanaged[Stdcall]<uint, int, int*, byte*, void>)pfn_glGetShaderSource)(shader, bufSize, length, source);
    
    private static nint pfn_glGetString;
    public static int* glGetString(uint name) => ((delegate* unmanaged[Stdcall]<uint, int*>)pfn_glGetString)(name);
    
    private static nint pfn_glGetTexParameterfv;
    public static void glGetTexParameterfv(uint target, uint pname, float* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, float*, void>)pfn_glGetTexParameterfv)(target, pname, @params);
    
    private static nint pfn_glGetTexParameteriv;
    public static void glGetTexParameteriv(uint target, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetTexParameteriv)(target, pname, @params);
    
    private static nint pfn_glGetUniformfv;
    public static void glGetUniformfv(uint program, int location, float* @params) => ((delegate* unmanaged[Stdcall]<uint, int, float*, void>)pfn_glGetUniformfv)(program, location, @params);
    
    private static nint pfn_glGetUniformiv;
    public static void glGetUniformiv(uint program, int location, int* @params) => ((delegate* unmanaged[Stdcall]<uint, int, int*, void>)pfn_glGetUniformiv)(program, location, @params);
    
    private static nint pfn_glGetUniformLocation;
    public static int glGetUniformLocation(uint program, byte* name) => ((delegate* unmanaged[Stdcall]<uint, byte*, int>)pfn_glGetUniformLocation)(program, name);
    
    private static nint pfn_glGetVertexAttribfv;
    public static void glGetVertexAttribfv(uint index, uint pname, float* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, float*, void>)pfn_glGetVertexAttribfv)(index, pname, @params);
    
    private static nint pfn_glGetVertexAttribiv;
    public static void glGetVertexAttribiv(uint index, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetVertexAttribiv)(index, pname, @params);
    
    private static nint pfn_glGetVertexAttribPointerv;
    public static void glGetVertexAttribPointerv(uint index, uint pname, void** pointer) => ((delegate* unmanaged[Stdcall]<uint, uint, void**, void>)pfn_glGetVertexAttribPointerv)(index, pname, pointer);
    
    private static nint pfn_glHint;
    public static void glHint(uint target, uint mode) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glHint)(target, mode);
    
    private static nint pfn_glIsBuffer;
    public static byte glIsBuffer(uint buffer) => ((delegate* unmanaged[Stdcall]<uint, byte>)pfn_glIsBuffer)(buffer);
    
    private static nint pfn_glIsEnabled;
    public static byte glIsEnabled(uint cap) => ((delegate* unmanaged[Stdcall]<uint, byte>)pfn_glIsEnabled)(cap);
    
    private static nint pfn_glIsFramebuffer;
    public static byte glIsFramebuffer(uint framebuffer) => ((delegate* unmanaged[Stdcall]<uint, byte>)pfn_glIsFramebuffer)(framebuffer);
    
    private static nint pfn_glIsProgram;
    public static byte glIsProgram(uint program) => ((delegate* unmanaged[Stdcall]<uint, byte>)pfn_glIsProgram)(program);
    
    private static nint pfn_glIsRenderbuffer;
    public static byte glIsRenderbuffer(uint renderbuffer) => ((delegate* unmanaged[Stdcall]<uint, byte>)pfn_glIsRenderbuffer)(renderbuffer);
    
    private static nint pfn_glIsShader;
    public static byte glIsShader(uint shader) => ((delegate* unmanaged[Stdcall]<uint, byte>)pfn_glIsShader)(shader);
    
    private static nint pfn_glIsTexture;
    public static byte glIsTexture(uint texture) => ((delegate* unmanaged[Stdcall]<uint, byte>)pfn_glIsTexture)(texture);
    
    private static nint pfn_glLineWidth;
    public static void glLineWidth(float width) => ((delegate* unmanaged[Stdcall]<float, void>)pfn_glLineWidth)(width);
    
    private static nint pfn_glLinkProgram;
    public static void glLinkProgram(uint program) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glLinkProgram)(program);
    
    private static nint pfn_glPixelStorei;
    public static void glPixelStorei(uint pname, int param) => ((delegate* unmanaged[Stdcall]<uint, int, void>)pfn_glPixelStorei)(pname, param);
    
    private static nint pfn_glPolygonOffset;
    public static void glPolygonOffset(float factor, float units) => ((delegate* unmanaged[Stdcall]<float, float, void>)pfn_glPolygonOffset)(factor, units);
    
    private static nint pfn_glReadPixels;
    public static void glReadPixels(int x, int y, int width, int height, uint format, uint type, void* pixels) => ((delegate* unmanaged[Stdcall]<int, int, int, int, uint, uint, void*, void>)pfn_glReadPixels)(x, y, width, height, format, type, pixels);
    
    private static nint pfn_glReleaseShaderCompiler;
    public static void glReleaseShaderCompiler() => ((delegate* unmanaged[Stdcall]<void>)pfn_glReleaseShaderCompiler)();
    
    private static nint pfn_glRenderbufferStorage;
    public static void glRenderbufferStorage(uint target, uint internalformat, int width, int height) => ((delegate* unmanaged[Stdcall]<uint, uint, int, int, void>)pfn_glRenderbufferStorage)(target, internalformat, width, height);
    
    private static nint pfn_glSampleCoverage;
    public static void glSampleCoverage(float value, byte invert) => ((delegate* unmanaged[Stdcall]<float, byte, void>)pfn_glSampleCoverage)(value, invert);
    
    private static nint pfn_glScissor;
    public static void glScissor(int x, int y, int width, int height) => ((delegate* unmanaged[Stdcall]<int, int, int, int, void>)pfn_glScissor)(x, y, width, height);
    
    private static nint pfn_glShaderBinary;
    public static void glShaderBinary(int count, uint* shaders, uint binaryformat, void* binary, int length) => ((delegate* unmanaged[Stdcall]<int, uint*, uint, void*, int, void>)pfn_glShaderBinary)(count, shaders, binaryformat, binary, length);
    
    private static nint pfn_glShaderSource;
    public static void glShaderSource(uint shader, int count, byte** @string, int* length) => ((delegate* unmanaged[Stdcall]<uint, int, byte**, int*, void>)pfn_glShaderSource)(shader, count, @string, length);
    
    private static nint pfn_glStencilFunc;
    public static void glStencilFunc(uint func, int @ref, uint mask) => ((delegate* unmanaged[Stdcall]<uint, int, uint, void>)pfn_glStencilFunc)(func, @ref, mask);
    
    private static nint pfn_glStencilFuncSeparate;
    public static void glStencilFuncSeparate(uint face, uint func, int @ref, uint mask) => ((delegate* unmanaged[Stdcall]<uint, uint, int, uint, void>)pfn_glStencilFuncSeparate)(face, func, @ref, mask);
    
    private static nint pfn_glStencilMask;
    public static void glStencilMask(uint mask) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glStencilMask)(mask);
    
    private static nint pfn_glStencilMaskSeparate;
    public static void glStencilMaskSeparate(uint face, uint mask) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glStencilMaskSeparate)(face, mask);
    
    private static nint pfn_glStencilOp;
    public static void glStencilOp(uint fail, uint zfail, uint zpass) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, void>)pfn_glStencilOp)(fail, zfail, zpass);
    
    private static nint pfn_glStencilOpSeparate;
    public static void glStencilOpSeparate(uint face, uint sfail, uint dpfail, uint dppass) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, uint, void>)pfn_glStencilOpSeparate)(face, sfail, dpfail, dppass);
    
    private static nint pfn_glTexImage2D;
    public static void glTexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, void* pixels) => ((delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, uint, uint, void*, void>)pfn_glTexImage2D)(target, level, internalformat, width, height, border, format, type, pixels);
    
    private static nint pfn_glTexParameterf;
    public static void glTexParameterf(uint target, uint pname, float param) => ((delegate* unmanaged[Stdcall]<uint, uint, float, void>)pfn_glTexParameterf)(target, pname, param);
    
    private static nint pfn_glTexParameterfv;
    public static void glTexParameterfv(uint target, uint pname, float* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, float*, void>)pfn_glTexParameterfv)(target, pname, @params);
    
    private static nint pfn_glTexParameteri;
    public static void glTexParameteri(uint target, uint pname, int param) => ((delegate* unmanaged[Stdcall]<uint, uint, int, void>)pfn_glTexParameteri)(target, pname, param);
    
    private static nint pfn_glTexParameteriv;
    public static void glTexParameteriv(uint target, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glTexParameteriv)(target, pname, @params);
    
    private static nint pfn_glTexSubImage2D;
    public static void glTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, void* pixels) => ((delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, uint, uint, void*, void>)pfn_glTexSubImage2D)(target, level, xoffset, yoffset, width, height, format, type, pixels);
    
    private static nint pfn_glUniform1f;
    public static void glUniform1f(int location, float v0) => ((delegate* unmanaged[Stdcall]<int, float, void>)pfn_glUniform1f)(location, v0);
    
    private static nint pfn_glUniform1fv;
    public static void glUniform1fv(int location, int count, float* value) => ((delegate* unmanaged[Stdcall]<int, int, float*, void>)pfn_glUniform1fv)(location, count, value);
    
    private static nint pfn_glUniform1i;
    public static void glUniform1i(int location, int v0) => ((delegate* unmanaged[Stdcall]<int, int, void>)pfn_glUniform1i)(location, v0);
    
    private static nint pfn_glUniform1iv;
    public static void glUniform1iv(int location, int count, int* value) => ((delegate* unmanaged[Stdcall]<int, int, int*, void>)pfn_glUniform1iv)(location, count, value);
    
    private static nint pfn_glUniform2f;
    public static void glUniform2f(int location, float v0, float v1) => ((delegate* unmanaged[Stdcall]<int, float, float, void>)pfn_glUniform2f)(location, v0, v1);
    
    private static nint pfn_glUniform2fv;
    public static void glUniform2fv(int location, int count, float* value) => ((delegate* unmanaged[Stdcall]<int, int, float*, void>)pfn_glUniform2fv)(location, count, value);
    
    private static nint pfn_glUniform2i;
    public static void glUniform2i(int location, int v0, int v1) => ((delegate* unmanaged[Stdcall]<int, int, int, void>)pfn_glUniform2i)(location, v0, v1);
    
    private static nint pfn_glUniform2iv;
    public static void glUniform2iv(int location, int count, int* value) => ((delegate* unmanaged[Stdcall]<int, int, int*, void>)pfn_glUniform2iv)(location, count, value);
    
    private static nint pfn_glUniform3f;
    public static void glUniform3f(int location, float v0, float v1, float v2) => ((delegate* unmanaged[Stdcall]<int, float, float, float, void>)pfn_glUniform3f)(location, v0, v1, v2);
    
    private static nint pfn_glUniform3fv;
    public static void glUniform3fv(int location, int count, float* value) => ((delegate* unmanaged[Stdcall]<int, int, float*, void>)pfn_glUniform3fv)(location, count, value);
    
    private static nint pfn_glUniform3i;
    public static void glUniform3i(int location, int v0, int v1, int v2) => ((delegate* unmanaged[Stdcall]<int, int, int, int, void>)pfn_glUniform3i)(location, v0, v1, v2);
    
    private static nint pfn_glUniform3iv;
    public static void glUniform3iv(int location, int count, int* value) => ((delegate* unmanaged[Stdcall]<int, int, int*, void>)pfn_glUniform3iv)(location, count, value);
    
    private static nint pfn_glUniform4f;
    public static void glUniform4f(int location, float v0, float v1, float v2, float v3) => ((delegate* unmanaged[Stdcall]<int, float, float, float, float, void>)pfn_glUniform4f)(location, v0, v1, v2, v3);
    
    private static nint pfn_glUniform4fv;
    public static void glUniform4fv(int location, int count, float* value) => ((delegate* unmanaged[Stdcall]<int, int, float*, void>)pfn_glUniform4fv)(location, count, value);
    
    private static nint pfn_glUniform4i;
    public static void glUniform4i(int location, int v0, int v1, int v2, int v3) => ((delegate* unmanaged[Stdcall]<int, int, int, int, int, void>)pfn_glUniform4i)(location, v0, v1, v2, v3);
    
    private static nint pfn_glUniform4iv;
    public static void glUniform4iv(int location, int count, int* value) => ((delegate* unmanaged[Stdcall]<int, int, int*, void>)pfn_glUniform4iv)(location, count, value);
    
    private static nint pfn_glUniformMatrix2fv;
    public static void glUniformMatrix2fv(int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<int, int, byte, float*, void>)pfn_glUniformMatrix2fv)(location, count, transpose, value);
    
    private static nint pfn_glUniformMatrix3fv;
    public static void glUniformMatrix3fv(int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<int, int, byte, float*, void>)pfn_glUniformMatrix3fv)(location, count, transpose, value);
    
    private static nint pfn_glUniformMatrix4fv;
    public static void glUniformMatrix4fv(int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<int, int, byte, float*, void>)pfn_glUniformMatrix4fv)(location, count, transpose, value);
    
    private static nint pfn_glUseProgram;
    public static void glUseProgram(uint program) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glUseProgram)(program);
    
    private static nint pfn_glValidateProgram;
    public static void glValidateProgram(uint program) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glValidateProgram)(program);
    
    private static nint pfn_glVertexAttrib1f;
    public static void glVertexAttrib1f(uint index, float x) => ((delegate* unmanaged[Stdcall]<uint, float, void>)pfn_glVertexAttrib1f)(index, x);
    
    private static nint pfn_glVertexAttrib1fv;
    public static void glVertexAttrib1fv(uint index, float* v) => ((delegate* unmanaged[Stdcall]<uint, float*, void>)pfn_glVertexAttrib1fv)(index, v);
    
    private static nint pfn_glVertexAttrib2f;
    public static void glVertexAttrib2f(uint index, float x, float y) => ((delegate* unmanaged[Stdcall]<uint, float, float, void>)pfn_glVertexAttrib2f)(index, x, y);
    
    private static nint pfn_glVertexAttrib2fv;
    public static void glVertexAttrib2fv(uint index, float* v) => ((delegate* unmanaged[Stdcall]<uint, float*, void>)pfn_glVertexAttrib2fv)(index, v);
    
    private static nint pfn_glVertexAttrib3f;
    public static void glVertexAttrib3f(uint index, float x, float y, float z) => ((delegate* unmanaged[Stdcall]<uint, float, float, float, void>)pfn_glVertexAttrib3f)(index, x, y, z);
    
    private static nint pfn_glVertexAttrib3fv;
    public static void glVertexAttrib3fv(uint index, float* v) => ((delegate* unmanaged[Stdcall]<uint, float*, void>)pfn_glVertexAttrib3fv)(index, v);
    
    private static nint pfn_glVertexAttrib4f;
    public static void glVertexAttrib4f(uint index, float x, float y, float z, float w) => ((delegate* unmanaged[Stdcall]<uint, float, float, float, float, void>)pfn_glVertexAttrib4f)(index, x, y, z, w);
    
    private static nint pfn_glVertexAttrib4fv;
    public static void glVertexAttrib4fv(uint index, float* v) => ((delegate* unmanaged[Stdcall]<uint, float*, void>)pfn_glVertexAttrib4fv)(index, v);
    
    private static nint pfn_glVertexAttribPointer;
    public static void glVertexAttribPointer(uint index, int size, uint type, byte normalized, int stride, void* pointer) => ((delegate* unmanaged[Stdcall]<uint, int, uint, byte, int, void*, void>)pfn_glVertexAttribPointer)(index, size, type, normalized, stride, pointer);
    
    private static nint pfn_glViewport;
    public static void glViewport(int x, int y, int width, int height) => ((delegate* unmanaged[Stdcall]<int, int, int, int, void>)pfn_glViewport)(x, y, width, height);
    
    private static nint pfn_glReadBuffer;
    public static void glReadBuffer(uint src) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glReadBuffer)(src);
    
    private static nint pfn_glDrawRangeElements;
    public static void glDrawRangeElements(uint mode, uint start, uint end, int count, uint type, void* indices) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint, void*, void>)pfn_glDrawRangeElements)(mode, start, end, count, type, indices);
    
    private static nint pfn_glTexImage3D;
    public static void glTexImage3D(uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, void* pixels) => ((delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, uint, uint, void*, void>)pfn_glTexImage3D)(target, level, internalformat, width, height, depth, border, format, type, pixels);
    
    private static nint pfn_glTexSubImage3D;
    public static void glTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, void* pixels) => ((delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, uint, uint, void*, void>)pfn_glTexSubImage3D)(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);
    
    private static nint pfn_glCopyTexSubImage3D;
    public static void glCopyTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => ((delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, int, void>)pfn_glCopyTexSubImage3D)(target, level, xoffset, yoffset, zoffset, x, y, width, height);
    
    private static nint pfn_glCompressedTexImage3D;
    public static void glCompressedTexImage3D(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, void* data) => ((delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, int, int, void*, void>)pfn_glCompressedTexImage3D)(target, level, internalformat, width, height, depth, border, imageSize, data);
    
    private static nint pfn_glCompressedTexSubImage3D;
    public static void glCompressedTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, void* data) => ((delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, int, int, uint, int, void*, void>)pfn_glCompressedTexSubImage3D)(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);
    
    private static nint pfn_glGenQueries;
    public static void glGenQueries(int n, uint* ids) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glGenQueries)(n, ids);
    
    private static nint pfn_glDeleteQueries;
    public static void glDeleteQueries(int n, uint* ids) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glDeleteQueries)(n, ids);
    
    private static nint pfn_glIsQuery;
    public static byte glIsQuery(uint id) => ((delegate* unmanaged[Stdcall]<uint, byte>)pfn_glIsQuery)(id);
    
    private static nint pfn_glBeginQuery;
    public static void glBeginQuery(uint target, uint id) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glBeginQuery)(target, id);
    
    private static nint pfn_glEndQuery;
    public static void glEndQuery(uint target) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glEndQuery)(target);
    
    private static nint pfn_glGetQueryiv;
    public static void glGetQueryiv(uint target, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetQueryiv)(target, pname, @params);
    
    private static nint pfn_glGetQueryObjectuiv;
    public static void glGetQueryObjectuiv(uint id, uint pname, uint* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, uint*, void>)pfn_glGetQueryObjectuiv)(id, pname, @params);
    
    private static nint pfn_glUnmapBuffer;
    public static byte glUnmapBuffer(uint target) => ((delegate* unmanaged[Stdcall]<uint, byte>)pfn_glUnmapBuffer)(target);
    
    private static nint pfn_glGetBufferPointerv;
    public static void glGetBufferPointerv(uint target, uint pname, void** @params) => ((delegate* unmanaged[Stdcall]<uint, uint, void**, void>)pfn_glGetBufferPointerv)(target, pname, @params);
    
    private static nint pfn_glDrawBuffers;
    public static void glDrawBuffers(int n, uint* bufs) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glDrawBuffers)(n, bufs);
    
    private static nint pfn_glUniformMatrix2x3fv;
    public static void glUniformMatrix2x3fv(int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<int, int, byte, float*, void>)pfn_glUniformMatrix2x3fv)(location, count, transpose, value);
    
    private static nint pfn_glUniformMatrix3x2fv;
    public static void glUniformMatrix3x2fv(int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<int, int, byte, float*, void>)pfn_glUniformMatrix3x2fv)(location, count, transpose, value);
    
    private static nint pfn_glUniformMatrix2x4fv;
    public static void glUniformMatrix2x4fv(int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<int, int, byte, float*, void>)pfn_glUniformMatrix2x4fv)(location, count, transpose, value);
    
    private static nint pfn_glUniformMatrix4x2fv;
    public static void glUniformMatrix4x2fv(int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<int, int, byte, float*, void>)pfn_glUniformMatrix4x2fv)(location, count, transpose, value);
    
    private static nint pfn_glUniformMatrix3x4fv;
    public static void glUniformMatrix3x4fv(int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<int, int, byte, float*, void>)pfn_glUniformMatrix3x4fv)(location, count, transpose, value);
    
    private static nint pfn_glUniformMatrix4x3fv;
    public static void glUniformMatrix4x3fv(int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<int, int, byte, float*, void>)pfn_glUniformMatrix4x3fv)(location, count, transpose, value);
    
    private static nint pfn_glBlitFramebuffer;
    public static void glBlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter) => ((delegate* unmanaged[Stdcall]<int, int, int, int, int, int, int, int, uint, uint, void>)pfn_glBlitFramebuffer)(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);
    
    private static nint pfn_glRenderbufferStorageMultisample;
    public static void glRenderbufferStorageMultisample(uint target, int samples, uint internalformat, int width, int height) => ((delegate* unmanaged[Stdcall]<uint, int, uint, int, int, void>)pfn_glRenderbufferStorageMultisample)(target, samples, internalformat, width, height);
    
    private static nint pfn_glFramebufferTextureLayer;
    public static void glFramebufferTextureLayer(uint target, uint attachment, uint texture, int level, int layer) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, int, int, void>)pfn_glFramebufferTextureLayer)(target, attachment, texture, level, layer);
    
    private static nint pfn_glMapBufferRange;
    public static void* glMapBufferRange(uint target, int offset, int length, uint access) => ((delegate* unmanaged[Stdcall]<uint, int, int, uint, void*>)pfn_glMapBufferRange)(target, offset, length, access);
    
    private static nint pfn_glFlushMappedBufferRange;
    public static void glFlushMappedBufferRange(uint target, int offset, int length) => ((delegate* unmanaged[Stdcall]<uint, int, int, void>)pfn_glFlushMappedBufferRange)(target, offset, length);
    
    private static nint pfn_glBindVertexArray;
    public static void glBindVertexArray(uint array) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glBindVertexArray)(array);
    
    private static nint pfn_glDeleteVertexArrays;
    public static void glDeleteVertexArrays(int n, uint* arrays) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glDeleteVertexArrays)(n, arrays);
    
    private static nint pfn_glGenVertexArrays;
    public static void glGenVertexArrays(int n, uint* arrays) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glGenVertexArrays)(n, arrays);
    
    private static nint pfn_glIsVertexArray;
    public static byte glIsVertexArray(uint array) => ((delegate* unmanaged[Stdcall]<uint, byte>)pfn_glIsVertexArray)(array);
    
    private static nint pfn_glGetIntegeri_v;
    public static void glGetIntegeri_v(uint target, uint index, int* data) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetIntegeri_v)(target, index, data);
    
    private static nint pfn_glBeginTransformFeedback;
    public static void glBeginTransformFeedback(uint primitiveMode) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glBeginTransformFeedback)(primitiveMode);
    
    private static nint pfn_glEndTransformFeedback;
    public static void glEndTransformFeedback() => ((delegate* unmanaged[Stdcall]<void>)pfn_glEndTransformFeedback)();
    
    private static nint pfn_glBindBufferRange;
    public static void glBindBufferRange(uint target, uint index, uint buffer, int offset, int size) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, int, int, void>)pfn_glBindBufferRange)(target, index, buffer, offset, size);
    
    private static nint pfn_glBindBufferBase;
    public static void glBindBufferBase(uint target, uint index, uint buffer) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, void>)pfn_glBindBufferBase)(target, index, buffer);
    
    private static nint pfn_glTransformFeedbackVaryings;
    public static void glTransformFeedbackVaryings(uint program, int count, byte** varyings, uint bufferMode) => ((delegate* unmanaged[Stdcall]<uint, int, byte**, uint, void>)pfn_glTransformFeedbackVaryings)(program, count, varyings, bufferMode);
    
    private static nint pfn_glGetTransformFeedbackVarying;
    public static void glGetTransformFeedbackVarying(uint program, uint index, int bufSize, int* length, int* size, uint* type, byte* name) => ((delegate* unmanaged[Stdcall]<uint, uint, int, int*, int*, uint*, byte*, void>)pfn_glGetTransformFeedbackVarying)(program, index, bufSize, length, size, type, name);
    
    private static nint pfn_glVertexAttribIPointer;
    public static void glVertexAttribIPointer(uint index, int size, uint type, int stride, void* pointer) => ((delegate* unmanaged[Stdcall]<uint, int, uint, int, void*, void>)pfn_glVertexAttribIPointer)(index, size, type, stride, pointer);
    
    private static nint pfn_glGetVertexAttribIiv;
    public static void glGetVertexAttribIiv(uint index, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetVertexAttribIiv)(index, pname, @params);
    
    private static nint pfn_glGetVertexAttribIuiv;
    public static void glGetVertexAttribIuiv(uint index, uint pname, uint* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, uint*, void>)pfn_glGetVertexAttribIuiv)(index, pname, @params);
    
    private static nint pfn_glVertexAttribI4i;
    public static void glVertexAttribI4i(uint index, int x, int y, int z, int w) => ((delegate* unmanaged[Stdcall]<uint, int, int, int, int, void>)pfn_glVertexAttribI4i)(index, x, y, z, w);
    
    private static nint pfn_glVertexAttribI4ui;
    public static void glVertexAttribI4ui(uint index, uint x, uint y, uint z, uint w) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, uint, uint, void>)pfn_glVertexAttribI4ui)(index, x, y, z, w);
    
    private static nint pfn_glVertexAttribI4iv;
    public static void glVertexAttribI4iv(uint index, int* v) => ((delegate* unmanaged[Stdcall]<uint, int*, void>)pfn_glVertexAttribI4iv)(index, v);
    
    private static nint pfn_glVertexAttribI4uiv;
    public static void glVertexAttribI4uiv(uint index, uint* v) => ((delegate* unmanaged[Stdcall]<uint, uint*, void>)pfn_glVertexAttribI4uiv)(index, v);
    
    private static nint pfn_glGetUniformuiv;
    public static void glGetUniformuiv(uint program, int location, uint* @params) => ((delegate* unmanaged[Stdcall]<uint, int, uint*, void>)pfn_glGetUniformuiv)(program, location, @params);
    
    private static nint pfn_glGetFragDataLocation;
    public static int glGetFragDataLocation(uint program, byte* name) => ((delegate* unmanaged[Stdcall]<uint, byte*, int>)pfn_glGetFragDataLocation)(program, name);
    
    private static nint pfn_glUniform1ui;
    public static void glUniform1ui(int location, uint v0) => ((delegate* unmanaged[Stdcall]<int, uint, void>)pfn_glUniform1ui)(location, v0);
    
    private static nint pfn_glUniform2ui;
    public static void glUniform2ui(int location, uint v0, uint v1) => ((delegate* unmanaged[Stdcall]<int, uint, uint, void>)pfn_glUniform2ui)(location, v0, v1);
    
    private static nint pfn_glUniform3ui;
    public static void glUniform3ui(int location, uint v0, uint v1, uint v2) => ((delegate* unmanaged[Stdcall]<int, uint, uint, uint, void>)pfn_glUniform3ui)(location, v0, v1, v2);
    
    private static nint pfn_glUniform4ui;
    public static void glUniform4ui(int location, uint v0, uint v1, uint v2, uint v3) => ((delegate* unmanaged[Stdcall]<int, uint, uint, uint, uint, void>)pfn_glUniform4ui)(location, v0, v1, v2, v3);
    
    private static nint pfn_glUniform1uiv;
    public static void glUniform1uiv(int location, int count, uint* value) => ((delegate* unmanaged[Stdcall]<int, int, uint*, void>)pfn_glUniform1uiv)(location, count, value);
    
    private static nint pfn_glUniform2uiv;
    public static void glUniform2uiv(int location, int count, uint* value) => ((delegate* unmanaged[Stdcall]<int, int, uint*, void>)pfn_glUniform2uiv)(location, count, value);
    
    private static nint pfn_glUniform3uiv;
    public static void glUniform3uiv(int location, int count, uint* value) => ((delegate* unmanaged[Stdcall]<int, int, uint*, void>)pfn_glUniform3uiv)(location, count, value);
    
    private static nint pfn_glUniform4uiv;
    public static void glUniform4uiv(int location, int count, uint* value) => ((delegate* unmanaged[Stdcall]<int, int, uint*, void>)pfn_glUniform4uiv)(location, count, value);
    
    private static nint pfn_glClearBufferiv;
    public static void glClearBufferiv(uint buffer, int drawbuffer, int* value) => ((delegate* unmanaged[Stdcall]<uint, int, int*, void>)pfn_glClearBufferiv)(buffer, drawbuffer, value);
    
    private static nint pfn_glClearBufferuiv;
    public static void glClearBufferuiv(uint buffer, int drawbuffer, uint* value) => ((delegate* unmanaged[Stdcall]<uint, int, uint*, void>)pfn_glClearBufferuiv)(buffer, drawbuffer, value);
    
    private static nint pfn_glClearBufferfv;
    public static void glClearBufferfv(uint buffer, int drawbuffer, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, float*, void>)pfn_glClearBufferfv)(buffer, drawbuffer, value);
    
    private static nint pfn_glClearBufferfi;
    public static void glClearBufferfi(uint buffer, int drawbuffer, float depth, int stencil) => ((delegate* unmanaged[Stdcall]<uint, int, float, int, void>)pfn_glClearBufferfi)(buffer, drawbuffer, depth, stencil);
    
    private static nint pfn_glGetStringi;
    public static int* glGetStringi(uint name, uint index) => ((delegate* unmanaged[Stdcall]<uint, uint, int*>)pfn_glGetStringi)(name, index);
    
    private static nint pfn_glCopyBufferSubData;
    public static void glCopyBufferSubData(uint readTarget, uint writeTarget, int readOffset, int writeOffset, int size) => ((delegate* unmanaged[Stdcall]<uint, uint, int, int, int, void>)pfn_glCopyBufferSubData)(readTarget, writeTarget, readOffset, writeOffset, size);
    
    private static nint pfn_glGetUniformIndices;
    public static void glGetUniformIndices(uint program, int uniformCount, byte** uniformNames, uint* uniformIndices) => ((delegate* unmanaged[Stdcall]<uint, int, byte**, uint*, void>)pfn_glGetUniformIndices)(program, uniformCount, uniformNames, uniformIndices);
    
    private static nint pfn_glGetActiveUniformsiv;
    public static void glGetActiveUniformsiv(uint program, int uniformCount, uint* uniformIndices, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, int, uint*, uint, int*, void>)pfn_glGetActiveUniformsiv)(program, uniformCount, uniformIndices, pname, @params);
    
    private static nint pfn_glGetUniformBlockIndex;
    public static uint glGetUniformBlockIndex(uint program, byte* uniformBlockName) => ((delegate* unmanaged[Stdcall]<uint, byte*, uint>)pfn_glGetUniformBlockIndex)(program, uniformBlockName);
    
    private static nint pfn_glGetActiveUniformBlockiv;
    public static void glGetActiveUniformBlockiv(uint program, uint uniformBlockIndex, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void>)pfn_glGetActiveUniformBlockiv)(program, uniformBlockIndex, pname, @params);
    
    private static nint pfn_glGetActiveUniformBlockName;
    public static void glGetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, int* length, byte* uniformBlockName) => ((delegate* unmanaged[Stdcall]<uint, uint, int, int*, byte*, void>)pfn_glGetActiveUniformBlockName)(program, uniformBlockIndex, bufSize, length, uniformBlockName);
    
    private static nint pfn_glUniformBlockBinding;
    public static void glUniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, void>)pfn_glUniformBlockBinding)(program, uniformBlockIndex, uniformBlockBinding);
    
    private static nint pfn_glDrawArraysInstanced;
    public static void glDrawArraysInstanced(uint mode, int first, int count, int instancecount) => ((delegate* unmanaged[Stdcall]<uint, int, int, int, void>)pfn_glDrawArraysInstanced)(mode, first, count, instancecount);
    
    private static nint pfn_glDrawElementsInstanced;
    public static void glDrawElementsInstanced(uint mode, int count, uint type, void* indices, int instancecount) => ((delegate* unmanaged[Stdcall]<uint, int, uint, void*, int, void>)pfn_glDrawElementsInstanced)(mode, count, type, indices, instancecount);
    
    private static nint pfn_glFenceSync;
    public static void* glFenceSync(uint condition, uint flags) => ((delegate* unmanaged[Stdcall]<uint, uint, void*>)pfn_glFenceSync)(condition, flags);
    
    private static nint pfn_glIsSync;
    public static byte glIsSync(void* sync) => ((delegate* unmanaged[Stdcall]<void*, byte>)pfn_glIsSync)(sync);
    
    private static nint pfn_glDeleteSync;
    public static void glDeleteSync(void* sync) => ((delegate* unmanaged[Stdcall]<void*, void>)pfn_glDeleteSync)(sync);
    
    private static nint pfn_glClientWaitSync;
    public static uint glClientWaitSync(void* sync, uint flags, int timeout) => ((delegate* unmanaged[Stdcall]<void*, uint, int, uint>)pfn_glClientWaitSync)(sync, flags, timeout);
    
    private static nint pfn_glWaitSync;
    public static void glWaitSync(void* sync, uint flags, int timeout) => ((delegate* unmanaged[Stdcall]<void*, uint, int, void>)pfn_glWaitSync)(sync, flags, timeout);
    
    private static nint pfn_glGetInteger64v;
    public static void glGetInteger64v(uint pname, int* data) => ((delegate* unmanaged[Stdcall]<uint, int*, void>)pfn_glGetInteger64v)(pname, data);
    
    private static nint pfn_glGetSynciv;
    public static void glGetSynciv(void* sync, uint pname, int bufSize, int* length, int* values) => ((delegate* unmanaged[Stdcall]<void*, uint, int, int*, int*, void>)pfn_glGetSynciv)(sync, pname, bufSize, length, values);
    
    private static nint pfn_glGetInteger64i_v;
    public static void glGetInteger64i_v(uint target, uint index, int* data) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetInteger64i_v)(target, index, data);
    
    private static nint pfn_glGetBufferParameteri64v;
    public static void glGetBufferParameteri64v(uint target, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetBufferParameteri64v)(target, pname, @params);
    
    private static nint pfn_glGenSamplers;
    public static void glGenSamplers(int count, uint* samplers) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glGenSamplers)(count, samplers);
    
    private static nint pfn_glDeleteSamplers;
    public static void glDeleteSamplers(int count, uint* samplers) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glDeleteSamplers)(count, samplers);
    
    private static nint pfn_glIsSampler;
    public static byte glIsSampler(uint sampler) => ((delegate* unmanaged[Stdcall]<uint, byte>)pfn_glIsSampler)(sampler);
    
    private static nint pfn_glBindSampler;
    public static void glBindSampler(uint unit, uint sampler) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glBindSampler)(unit, sampler);
    
    private static nint pfn_glSamplerParameteri;
    public static void glSamplerParameteri(uint sampler, uint pname, int param) => ((delegate* unmanaged[Stdcall]<uint, uint, int, void>)pfn_glSamplerParameteri)(sampler, pname, param);
    
    private static nint pfn_glSamplerParameteriv;
    public static void glSamplerParameteriv(uint sampler, uint pname, int* param) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glSamplerParameteriv)(sampler, pname, param);
    
    private static nint pfn_glSamplerParameterf;
    public static void glSamplerParameterf(uint sampler, uint pname, float param) => ((delegate* unmanaged[Stdcall]<uint, uint, float, void>)pfn_glSamplerParameterf)(sampler, pname, param);
    
    private static nint pfn_glSamplerParameterfv;
    public static void glSamplerParameterfv(uint sampler, uint pname, float* param) => ((delegate* unmanaged[Stdcall]<uint, uint, float*, void>)pfn_glSamplerParameterfv)(sampler, pname, param);
    
    private static nint pfn_glGetSamplerParameteriv;
    public static void glGetSamplerParameteriv(uint sampler, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetSamplerParameteriv)(sampler, pname, @params);
    
    private static nint pfn_glGetSamplerParameterfv;
    public static void glGetSamplerParameterfv(uint sampler, uint pname, float* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, float*, void>)pfn_glGetSamplerParameterfv)(sampler, pname, @params);
    
    private static nint pfn_glVertexAttribDivisor;
    public static void glVertexAttribDivisor(uint index, uint divisor) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glVertexAttribDivisor)(index, divisor);
    
    private static nint pfn_glBindTransformFeedback;
    public static void glBindTransformFeedback(uint target, uint id) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glBindTransformFeedback)(target, id);
    
    private static nint pfn_glDeleteTransformFeedbacks;
    public static void glDeleteTransformFeedbacks(int n, uint* ids) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glDeleteTransformFeedbacks)(n, ids);
    
    private static nint pfn_glGenTransformFeedbacks;
    public static void glGenTransformFeedbacks(int n, uint* ids) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glGenTransformFeedbacks)(n, ids);
    
    private static nint pfn_glIsTransformFeedback;
    public static byte glIsTransformFeedback(uint id) => ((delegate* unmanaged[Stdcall]<uint, byte>)pfn_glIsTransformFeedback)(id);
    
    private static nint pfn_glPauseTransformFeedback;
    public static void glPauseTransformFeedback() => ((delegate* unmanaged[Stdcall]<void>)pfn_glPauseTransformFeedback)();
    
    private static nint pfn_glResumeTransformFeedback;
    public static void glResumeTransformFeedback() => ((delegate* unmanaged[Stdcall]<void>)pfn_glResumeTransformFeedback)();
    
    private static nint pfn_glGetProgramBinary;
    public static void glGetProgramBinary(uint program, int bufSize, int* length, uint* binaryFormat, void* binary) => ((delegate* unmanaged[Stdcall]<uint, int, int*, uint*, void*, void>)pfn_glGetProgramBinary)(program, bufSize, length, binaryFormat, binary);
    
    private static nint pfn_glProgramBinary;
    public static void glProgramBinary(uint program, uint binaryFormat, void* binary, int length) => ((delegate* unmanaged[Stdcall]<uint, uint, void*, int, void>)pfn_glProgramBinary)(program, binaryFormat, binary, length);
    
    private static nint pfn_glProgramParameteri;
    public static void glProgramParameteri(uint program, uint pname, int value) => ((delegate* unmanaged[Stdcall]<uint, uint, int, void>)pfn_glProgramParameteri)(program, pname, value);
    
    private static nint pfn_glInvalidateFramebuffer;
    public static void glInvalidateFramebuffer(uint target, int numAttachments, uint* attachments) => ((delegate* unmanaged[Stdcall]<uint, int, uint*, void>)pfn_glInvalidateFramebuffer)(target, numAttachments, attachments);
    
    private static nint pfn_glInvalidateSubFramebuffer;
    public static void glInvalidateSubFramebuffer(uint target, int numAttachments, uint* attachments, int x, int y, int width, int height) => ((delegate* unmanaged[Stdcall]<uint, int, uint*, int, int, int, int, void>)pfn_glInvalidateSubFramebuffer)(target, numAttachments, attachments, x, y, width, height);
    
    private static nint pfn_glTexStorage2D;
    public static void glTexStorage2D(uint target, int levels, uint internalformat, int width, int height) => ((delegate* unmanaged[Stdcall]<uint, int, uint, int, int, void>)pfn_glTexStorage2D)(target, levels, internalformat, width, height);
    
    private static nint pfn_glTexStorage3D;
    public static void glTexStorage3D(uint target, int levels, uint internalformat, int width, int height, int depth) => ((delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, void>)pfn_glTexStorage3D)(target, levels, internalformat, width, height, depth);
    
    private static nint pfn_glGetInternalformativ;
    public static void glGetInternalformativ(uint target, uint internalformat, uint pname, int bufSize, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, int, int*, void>)pfn_glGetInternalformativ)(target, internalformat, pname, bufSize, @params);
    
    private static nint pfn_glDispatchCompute;
    public static void glDispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, void>)pfn_glDispatchCompute)(num_groups_x, num_groups_y, num_groups_z);
    
    private static nint pfn_glDispatchComputeIndirect;
    public static void glDispatchComputeIndirect(int indirect) => ((delegate* unmanaged[Stdcall]<int, void>)pfn_glDispatchComputeIndirect)(indirect);
    
    private static nint pfn_glDrawArraysIndirect;
    public static void glDrawArraysIndirect(uint mode, void* indirect) => ((delegate* unmanaged[Stdcall]<uint, void*, void>)pfn_glDrawArraysIndirect)(mode, indirect);
    
    private static nint pfn_glDrawElementsIndirect;
    public static void glDrawElementsIndirect(uint mode, uint type, void* indirect) => ((delegate* unmanaged[Stdcall]<uint, uint, void*, void>)pfn_glDrawElementsIndirect)(mode, type, indirect);
    
    private static nint pfn_glFramebufferParameteri;
    public static void glFramebufferParameteri(uint target, uint pname, int param) => ((delegate* unmanaged[Stdcall]<uint, uint, int, void>)pfn_glFramebufferParameteri)(target, pname, param);
    
    private static nint pfn_glGetFramebufferParameteriv;
    public static void glGetFramebufferParameteriv(uint target, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetFramebufferParameteriv)(target, pname, @params);
    
    private static nint pfn_glGetProgramInterfaceiv;
    public static void glGetProgramInterfaceiv(uint program, uint programInterface, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, int*, void>)pfn_glGetProgramInterfaceiv)(program, programInterface, pname, @params);
    
    private static nint pfn_glGetProgramResourceIndex;
    public static uint glGetProgramResourceIndex(uint program, uint programInterface, byte* name) => ((delegate* unmanaged[Stdcall]<uint, uint, byte*, uint>)pfn_glGetProgramResourceIndex)(program, programInterface, name);
    
    private static nint pfn_glGetProgramResourceName;
    public static void glGetProgramResourceName(uint program, uint programInterface, uint index, int bufSize, int* length, byte* name) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, int, int*, byte*, void>)pfn_glGetProgramResourceName)(program, programInterface, index, bufSize, length, name);
    
    private static nint pfn_glGetProgramResourceiv;
    public static void glGetProgramResourceiv(uint program, uint programInterface, uint index, int propCount, uint* props, int bufSize, int* length, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint*, int, int*, int*, void>)pfn_glGetProgramResourceiv)(program, programInterface, index, propCount, props, bufSize, length, @params);
    
    private static nint pfn_glGetProgramResourceLocation;
    public static int glGetProgramResourceLocation(uint program, uint programInterface, byte* name) => ((delegate* unmanaged[Stdcall]<uint, uint, byte*, int>)pfn_glGetProgramResourceLocation)(program, programInterface, name);
    
    private static nint pfn_glUseProgramStages;
    public static void glUseProgramStages(uint pipeline, uint stages, uint program) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, void>)pfn_glUseProgramStages)(pipeline, stages, program);
    
    private static nint pfn_glActiveShaderProgram;
    public static void glActiveShaderProgram(uint pipeline, uint program) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glActiveShaderProgram)(pipeline, program);
    
    private static nint pfn_glCreateShaderProgramv;
    public static uint glCreateShaderProgramv(uint type, int count, byte** strings) => ((delegate* unmanaged[Stdcall]<uint, int, byte**, uint>)pfn_glCreateShaderProgramv)(type, count, strings);
    
    private static nint pfn_glBindProgramPipeline;
    public static void glBindProgramPipeline(uint pipeline) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glBindProgramPipeline)(pipeline);
    
    private static nint pfn_glDeleteProgramPipelines;
    public static void glDeleteProgramPipelines(int n, uint* pipelines) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glDeleteProgramPipelines)(n, pipelines);
    
    private static nint pfn_glGenProgramPipelines;
    public static void glGenProgramPipelines(int n, uint* pipelines) => ((delegate* unmanaged[Stdcall]<int, uint*, void>)pfn_glGenProgramPipelines)(n, pipelines);
    
    private static nint pfn_glIsProgramPipeline;
    public static byte glIsProgramPipeline(uint pipeline) => ((delegate* unmanaged[Stdcall]<uint, byte>)pfn_glIsProgramPipeline)(pipeline);
    
    private static nint pfn_glGetProgramPipelineiv;
    public static void glGetProgramPipelineiv(uint pipeline, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetProgramPipelineiv)(pipeline, pname, @params);
    
    private static nint pfn_glProgramUniform1i;
    public static void glProgramUniform1i(uint program, int location, int v0) => ((delegate* unmanaged[Stdcall]<uint, int, int, void>)pfn_glProgramUniform1i)(program, location, v0);
    
    private static nint pfn_glProgramUniform2i;
    public static void glProgramUniform2i(uint program, int location, int v0, int v1) => ((delegate* unmanaged[Stdcall]<uint, int, int, int, void>)pfn_glProgramUniform2i)(program, location, v0, v1);
    
    private static nint pfn_glProgramUniform3i;
    public static void glProgramUniform3i(uint program, int location, int v0, int v1, int v2) => ((delegate* unmanaged[Stdcall]<uint, int, int, int, int, void>)pfn_glProgramUniform3i)(program, location, v0, v1, v2);
    
    private static nint pfn_glProgramUniform4i;
    public static void glProgramUniform4i(uint program, int location, int v0, int v1, int v2, int v3) => ((delegate* unmanaged[Stdcall]<uint, int, int, int, int, int, void>)pfn_glProgramUniform4i)(program, location, v0, v1, v2, v3);
    
    private static nint pfn_glProgramUniform1ui;
    public static void glProgramUniform1ui(uint program, int location, uint v0) => ((delegate* unmanaged[Stdcall]<uint, int, uint, void>)pfn_glProgramUniform1ui)(program, location, v0);
    
    private static nint pfn_glProgramUniform2ui;
    public static void glProgramUniform2ui(uint program, int location, uint v0, uint v1) => ((delegate* unmanaged[Stdcall]<uint, int, uint, uint, void>)pfn_glProgramUniform2ui)(program, location, v0, v1);
    
    private static nint pfn_glProgramUniform3ui;
    public static void glProgramUniform3ui(uint program, int location, uint v0, uint v1, uint v2) => ((delegate* unmanaged[Stdcall]<uint, int, uint, uint, uint, void>)pfn_glProgramUniform3ui)(program, location, v0, v1, v2);
    
    private static nint pfn_glProgramUniform4ui;
    public static void glProgramUniform4ui(uint program, int location, uint v0, uint v1, uint v2, uint v3) => ((delegate* unmanaged[Stdcall]<uint, int, uint, uint, uint, uint, void>)pfn_glProgramUniform4ui)(program, location, v0, v1, v2, v3);
    
    private static nint pfn_glProgramUniform1f;
    public static void glProgramUniform1f(uint program, int location, float v0) => ((delegate* unmanaged[Stdcall]<uint, int, float, void>)pfn_glProgramUniform1f)(program, location, v0);
    
    private static nint pfn_glProgramUniform2f;
    public static void glProgramUniform2f(uint program, int location, float v0, float v1) => ((delegate* unmanaged[Stdcall]<uint, int, float, float, void>)pfn_glProgramUniform2f)(program, location, v0, v1);
    
    private static nint pfn_glProgramUniform3f;
    public static void glProgramUniform3f(uint program, int location, float v0, float v1, float v2) => ((delegate* unmanaged[Stdcall]<uint, int, float, float, float, void>)pfn_glProgramUniform3f)(program, location, v0, v1, v2);
    
    private static nint pfn_glProgramUniform4f;
    public static void glProgramUniform4f(uint program, int location, float v0, float v1, float v2, float v3) => ((delegate* unmanaged[Stdcall]<uint, int, float, float, float, float, void>)pfn_glProgramUniform4f)(program, location, v0, v1, v2, v3);
    
    private static nint pfn_glProgramUniform1iv;
    public static void glProgramUniform1iv(uint program, int location, int count, int* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, int*, void>)pfn_glProgramUniform1iv)(program, location, count, value);
    
    private static nint pfn_glProgramUniform2iv;
    public static void glProgramUniform2iv(uint program, int location, int count, int* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, int*, void>)pfn_glProgramUniform2iv)(program, location, count, value);
    
    private static nint pfn_glProgramUniform3iv;
    public static void glProgramUniform3iv(uint program, int location, int count, int* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, int*, void>)pfn_glProgramUniform3iv)(program, location, count, value);
    
    private static nint pfn_glProgramUniform4iv;
    public static void glProgramUniform4iv(uint program, int location, int count, int* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, int*, void>)pfn_glProgramUniform4iv)(program, location, count, value);
    
    private static nint pfn_glProgramUniform1uiv;
    public static void glProgramUniform1uiv(uint program, int location, int count, uint* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, uint*, void>)pfn_glProgramUniform1uiv)(program, location, count, value);
    
    private static nint pfn_glProgramUniform2uiv;
    public static void glProgramUniform2uiv(uint program, int location, int count, uint* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, uint*, void>)pfn_glProgramUniform2uiv)(program, location, count, value);
    
    private static nint pfn_glProgramUniform3uiv;
    public static void glProgramUniform3uiv(uint program, int location, int count, uint* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, uint*, void>)pfn_glProgramUniform3uiv)(program, location, count, value);
    
    private static nint pfn_glProgramUniform4uiv;
    public static void glProgramUniform4uiv(uint program, int location, int count, uint* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, uint*, void>)pfn_glProgramUniform4uiv)(program, location, count, value);
    
    private static nint pfn_glProgramUniform1fv;
    public static void glProgramUniform1fv(uint program, int location, int count, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, float*, void>)pfn_glProgramUniform1fv)(program, location, count, value);
    
    private static nint pfn_glProgramUniform2fv;
    public static void glProgramUniform2fv(uint program, int location, int count, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, float*, void>)pfn_glProgramUniform2fv)(program, location, count, value);
    
    private static nint pfn_glProgramUniform3fv;
    public static void glProgramUniform3fv(uint program, int location, int count, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, float*, void>)pfn_glProgramUniform3fv)(program, location, count, value);
    
    private static nint pfn_glProgramUniform4fv;
    public static void glProgramUniform4fv(uint program, int location, int count, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, float*, void>)pfn_glProgramUniform4fv)(program, location, count, value);
    
    private static nint pfn_glProgramUniformMatrix2fv;
    public static void glProgramUniformMatrix2fv(uint program, int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void>)pfn_glProgramUniformMatrix2fv)(program, location, count, transpose, value);
    
    private static nint pfn_glProgramUniformMatrix3fv;
    public static void glProgramUniformMatrix3fv(uint program, int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void>)pfn_glProgramUniformMatrix3fv)(program, location, count, transpose, value);
    
    private static nint pfn_glProgramUniformMatrix4fv;
    public static void glProgramUniformMatrix4fv(uint program, int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void>)pfn_glProgramUniformMatrix4fv)(program, location, count, transpose, value);
    
    private static nint pfn_glProgramUniformMatrix2x3fv;
    public static void glProgramUniformMatrix2x3fv(uint program, int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void>)pfn_glProgramUniformMatrix2x3fv)(program, location, count, transpose, value);
    
    private static nint pfn_glProgramUniformMatrix3x2fv;
    public static void glProgramUniformMatrix3x2fv(uint program, int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void>)pfn_glProgramUniformMatrix3x2fv)(program, location, count, transpose, value);
    
    private static nint pfn_glProgramUniformMatrix2x4fv;
    public static void glProgramUniformMatrix2x4fv(uint program, int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void>)pfn_glProgramUniformMatrix2x4fv)(program, location, count, transpose, value);
    
    private static nint pfn_glProgramUniformMatrix4x2fv;
    public static void glProgramUniformMatrix4x2fv(uint program, int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void>)pfn_glProgramUniformMatrix4x2fv)(program, location, count, transpose, value);
    
    private static nint pfn_glProgramUniformMatrix3x4fv;
    public static void glProgramUniformMatrix3x4fv(uint program, int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void>)pfn_glProgramUniformMatrix3x4fv)(program, location, count, transpose, value);
    
    private static nint pfn_glProgramUniformMatrix4x3fv;
    public static void glProgramUniformMatrix4x3fv(uint program, int location, int count, byte transpose, float* value) => ((delegate* unmanaged[Stdcall]<uint, int, int, byte, float*, void>)pfn_glProgramUniformMatrix4x3fv)(program, location, count, transpose, value);
    
    private static nint pfn_glValidateProgramPipeline;
    public static void glValidateProgramPipeline(uint pipeline) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glValidateProgramPipeline)(pipeline);
    
    private static nint pfn_glGetProgramPipelineInfoLog;
    public static void glGetProgramPipelineInfoLog(uint pipeline, int bufSize, int* length, byte* infoLog) => ((delegate* unmanaged[Stdcall]<uint, int, int*, byte*, void>)pfn_glGetProgramPipelineInfoLog)(pipeline, bufSize, length, infoLog);
    
    private static nint pfn_glBindImageTexture;
    public static void glBindImageTexture(uint unit, uint texture, int level, byte layered, int layer, uint access, uint format) => ((delegate* unmanaged[Stdcall]<uint, uint, int, byte, int, uint, uint, void>)pfn_glBindImageTexture)(unit, texture, level, layered, layer, access, format);
    
    private static nint pfn_glGetBooleani_v;
    public static void glGetBooleani_v(uint target, uint index, byte* data) => ((delegate* unmanaged[Stdcall]<uint, uint, byte*, void>)pfn_glGetBooleani_v)(target, index, data);
    
    private static nint pfn_glMemoryBarrier;
    public static void glMemoryBarrier(uint barriers) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glMemoryBarrier)(barriers);
    
    private static nint pfn_glMemoryBarrierByRegion;
    public static void glMemoryBarrierByRegion(uint barriers) => ((delegate* unmanaged[Stdcall]<uint, void>)pfn_glMemoryBarrierByRegion)(barriers);
    
    private static nint pfn_glTexStorage2DMultisample;
    public static void glTexStorage2DMultisample(uint target, int samples, uint internalformat, int width, int height, byte fixedsamplelocations) => ((delegate* unmanaged[Stdcall]<uint, int, uint, int, int, byte, void>)pfn_glTexStorage2DMultisample)(target, samples, internalformat, width, height, fixedsamplelocations);
    
    private static nint pfn_glGetMultisamplefv;
    public static void glGetMultisamplefv(uint pname, uint index, float* val) => ((delegate* unmanaged[Stdcall]<uint, uint, float*, void>)pfn_glGetMultisamplefv)(pname, index, val);
    
    private static nint pfn_glSampleMaski;
    public static void glSampleMaski(uint maskNumber, uint mask) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glSampleMaski)(maskNumber, mask);
    
    private static nint pfn_glGetTexLevelParameteriv;
    public static void glGetTexLevelParameteriv(uint target, int level, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, int, uint, int*, void>)pfn_glGetTexLevelParameteriv)(target, level, pname, @params);
    
    private static nint pfn_glGetTexLevelParameterfv;
    public static void glGetTexLevelParameterfv(uint target, int level, uint pname, float* @params) => ((delegate* unmanaged[Stdcall]<uint, int, uint, float*, void>)pfn_glGetTexLevelParameterfv)(target, level, pname, @params);
    
    private static nint pfn_glBindVertexBuffer;
    public static void glBindVertexBuffer(uint bindingindex, uint buffer, int offset, int stride) => ((delegate* unmanaged[Stdcall]<uint, uint, int, int, void>)pfn_glBindVertexBuffer)(bindingindex, buffer, offset, stride);
    
    private static nint pfn_glVertexAttribFormat;
    public static void glVertexAttribFormat(uint attribindex, int size, uint type, byte normalized, uint relativeoffset) => ((delegate* unmanaged[Stdcall]<uint, int, uint, byte, uint, void>)pfn_glVertexAttribFormat)(attribindex, size, type, normalized, relativeoffset);
    
    private static nint pfn_glVertexAttribIFormat;
    public static void glVertexAttribIFormat(uint attribindex, int size, uint type, uint relativeoffset) => ((delegate* unmanaged[Stdcall]<uint, int, uint, uint, void>)pfn_glVertexAttribIFormat)(attribindex, size, type, relativeoffset);
    
    private static nint pfn_glVertexAttribBinding;
    public static void glVertexAttribBinding(uint attribindex, uint bindingindex) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glVertexAttribBinding)(attribindex, bindingindex);
    
    private static nint pfn_glVertexBindingDivisor;
    public static void glVertexBindingDivisor(uint bindingindex, uint divisor) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glVertexBindingDivisor)(bindingindex, divisor);
    
    private static nint pfn_glBlendBarrier;
    public static void glBlendBarrier() => ((delegate* unmanaged[Stdcall]<void>)pfn_glBlendBarrier)();
    
    private static nint pfn_glCopyImageSubData;
    public static void glCopyImageSubData(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth) => ((delegate* unmanaged[Stdcall]<uint, uint, int, int, int, int, uint, uint, int, int, int, int, int, int, int, void>)pfn_glCopyImageSubData)(srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth);
    
    private static nint pfn_glDebugMessageControl;
    public static void glDebugMessageControl(uint source, uint type, uint severity, int count, uint* ids, byte enabled) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint*, byte, void>)pfn_glDebugMessageControl)(source, type, severity, count, ids, enabled);
    
    private static nint pfn_glDebugMessageInsert;
    public static void glDebugMessageInsert(uint source, uint type, uint id, uint severity, int length, byte* buf) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, uint, int, byte*, void>)pfn_glDebugMessageInsert)(source, type, id, severity, length, buf);
    
    private static nint pfn_glDebugMessageCallback;
    public static void glDebugMessageCallback(delegate* unmanaged[Stdcall]<uint, uint, uint, uint, int, byte*, void*, void> callback, void* userParam) => ((delegate* unmanaged[Stdcall]<delegate* unmanaged[Stdcall]<uint, uint, uint, uint, int, byte*, void*, void>, void*, void>)pfn_glDebugMessageCallback)(callback, userParam);
    
    private static nint pfn_glGetDebugMessageLog;
    public static uint glGetDebugMessageLog(uint count, int bufSize, uint* sources, uint* types, uint* ids, uint* severities, int* lengths, byte* messageLog) => ((delegate* unmanaged[Stdcall]<uint, int, uint*, uint*, uint*, uint*, int*, byte*, uint>)pfn_glGetDebugMessageLog)(count, bufSize, sources, types, ids, severities, lengths, messageLog);
    
    private static nint pfn_glPushDebugGroup;
    public static void glPushDebugGroup(uint source, uint id, int length, byte* message) => ((delegate* unmanaged[Stdcall]<uint, uint, int, byte*, void>)pfn_glPushDebugGroup)(source, id, length, message);
    
    private static nint pfn_glPopDebugGroup;
    public static void glPopDebugGroup() => ((delegate* unmanaged[Stdcall]<void>)pfn_glPopDebugGroup)();
    
    private static nint pfn_glObjectLabel;
    public static void glObjectLabel(uint identifier, uint name, int length, byte* label) => ((delegate* unmanaged[Stdcall]<uint, uint, int, byte*, void>)pfn_glObjectLabel)(identifier, name, length, label);
    
    private static nint pfn_glGetObjectLabel;
    public static void glGetObjectLabel(uint identifier, uint name, int bufSize, int* length, byte* label) => ((delegate* unmanaged[Stdcall]<uint, uint, int, int*, byte*, void>)pfn_glGetObjectLabel)(identifier, name, bufSize, length, label);
    
    private static nint pfn_glObjectPtrLabel;
    public static void glObjectPtrLabel(void* ptr, int length, byte* label) => ((delegate* unmanaged[Stdcall]<void*, int, byte*, void>)pfn_glObjectPtrLabel)(ptr, length, label);
    
    private static nint pfn_glGetObjectPtrLabel;
    public static void glGetObjectPtrLabel(void* ptr, int bufSize, int* length, byte* label) => ((delegate* unmanaged[Stdcall]<void*, int, int*, byte*, void>)pfn_glGetObjectPtrLabel)(ptr, bufSize, length, label);
    
    private static nint pfn_glGetPointerv;
    public static void glGetPointerv(uint pname, void** @params) => ((delegate* unmanaged[Stdcall]<uint, void**, void>)pfn_glGetPointerv)(pname, @params);
    
    private static nint pfn_glEnablei;
    public static void glEnablei(uint target, uint index) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glEnablei)(target, index);
    
    private static nint pfn_glDisablei;
    public static void glDisablei(uint target, uint index) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glDisablei)(target, index);
    
    private static nint pfn_glBlendEquationi;
    public static void glBlendEquationi(uint buf, uint mode) => ((delegate* unmanaged[Stdcall]<uint, uint, void>)pfn_glBlendEquationi)(buf, mode);
    
    private static nint pfn_glBlendEquationSeparatei;
    public static void glBlendEquationSeparatei(uint buf, uint modeRGB, uint modeAlpha) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, void>)pfn_glBlendEquationSeparatei)(buf, modeRGB, modeAlpha);
    
    private static nint pfn_glBlendFunci;
    public static void glBlendFunci(uint buf, uint src, uint dst) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, void>)pfn_glBlendFunci)(buf, src, dst);
    
    private static nint pfn_glBlendFuncSeparatei;
    public static void glBlendFuncSeparatei(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, uint, uint, void>)pfn_glBlendFuncSeparatei)(buf, srcRGB, dstRGB, srcAlpha, dstAlpha);
    
    private static nint pfn_glColorMaski;
    public static void glColorMaski(uint index, byte r, byte g, byte b, byte a) => ((delegate* unmanaged[Stdcall]<uint, byte, byte, byte, byte, void>)pfn_glColorMaski)(index, r, g, b, a);
    
    private static nint pfn_glIsEnabledi;
    public static byte glIsEnabledi(uint target, uint index) => ((delegate* unmanaged[Stdcall]<uint, uint, byte>)pfn_glIsEnabledi)(target, index);
    
    private static nint pfn_glDrawElementsBaseVertex;
    public static void glDrawElementsBaseVertex(uint mode, int count, uint type, void* indices, int basevertex) => ((delegate* unmanaged[Stdcall]<uint, int, uint, void*, int, void>)pfn_glDrawElementsBaseVertex)(mode, count, type, indices, basevertex);
    
    private static nint pfn_glDrawRangeElementsBaseVertex;
    public static void glDrawRangeElementsBaseVertex(uint mode, uint start, uint end, int count, uint type, void* indices, int basevertex) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, int, uint, void*, int, void>)pfn_glDrawRangeElementsBaseVertex)(mode, start, end, count, type, indices, basevertex);
    
    private static nint pfn_glDrawElementsInstancedBaseVertex;
    public static void glDrawElementsInstancedBaseVertex(uint mode, int count, uint type, void* indices, int instancecount, int basevertex) => ((delegate* unmanaged[Stdcall]<uint, int, uint, void*, int, int, void>)pfn_glDrawElementsInstancedBaseVertex)(mode, count, type, indices, instancecount, basevertex);
    
    private static nint pfn_glFramebufferTexture;
    public static void glFramebufferTexture(uint target, uint attachment, uint texture, int level) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, int, void>)pfn_glFramebufferTexture)(target, attachment, texture, level);
    
    private static nint pfn_glPrimitiveBoundingBox;
    public static void glPrimitiveBoundingBox(float minX, float minY, float minZ, float minW, float maxX, float maxY, float maxZ, float maxW) => ((delegate* unmanaged[Stdcall]<float, float, float, float, float, float, float, float, void>)pfn_glPrimitiveBoundingBox)(minX, minY, minZ, minW, maxX, maxY, maxZ, maxW);
    
    private static nint pfn_glGetGraphicsResetStatus;
    public static uint glGetGraphicsResetStatus() => ((delegate* unmanaged[Stdcall]<uint>)pfn_glGetGraphicsResetStatus)();
    
    private static nint pfn_glReadnPixels;
    public static void glReadnPixels(int x, int y, int width, int height, uint format, uint type, int bufSize, void* data) => ((delegate* unmanaged[Stdcall]<int, int, int, int, uint, uint, int, void*, void>)pfn_glReadnPixels)(x, y, width, height, format, type, bufSize, data);
    
    private static nint pfn_glGetnUniformfv;
    public static void glGetnUniformfv(uint program, int location, int bufSize, float* @params) => ((delegate* unmanaged[Stdcall]<uint, int, int, float*, void>)pfn_glGetnUniformfv)(program, location, bufSize, @params);
    
    private static nint pfn_glGetnUniformiv;
    public static void glGetnUniformiv(uint program, int location, int bufSize, int* @params) => ((delegate* unmanaged[Stdcall]<uint, int, int, int*, void>)pfn_glGetnUniformiv)(program, location, bufSize, @params);
    
    private static nint pfn_glGetnUniformuiv;
    public static void glGetnUniformuiv(uint program, int location, int bufSize, uint* @params) => ((delegate* unmanaged[Stdcall]<uint, int, int, uint*, void>)pfn_glGetnUniformuiv)(program, location, bufSize, @params);
    
    private static nint pfn_glMinSampleShading;
    public static void glMinSampleShading(float value) => ((delegate* unmanaged[Stdcall]<float, void>)pfn_glMinSampleShading)(value);
    
    private static nint pfn_glPatchParameteri;
    public static void glPatchParameteri(uint pname, int value) => ((delegate* unmanaged[Stdcall]<uint, int, void>)pfn_glPatchParameteri)(pname, value);
    
    private static nint pfn_glTexParameterIiv;
    public static void glTexParameterIiv(uint target, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glTexParameterIiv)(target, pname, @params);
    
    private static nint pfn_glTexParameterIuiv;
    public static void glTexParameterIuiv(uint target, uint pname, uint* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, uint*, void>)pfn_glTexParameterIuiv)(target, pname, @params);
    
    private static nint pfn_glGetTexParameterIiv;
    public static void glGetTexParameterIiv(uint target, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetTexParameterIiv)(target, pname, @params);
    
    private static nint pfn_glGetTexParameterIuiv;
    public static void glGetTexParameterIuiv(uint target, uint pname, uint* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, uint*, void>)pfn_glGetTexParameterIuiv)(target, pname, @params);
    
    private static nint pfn_glSamplerParameterIiv;
    public static void glSamplerParameterIiv(uint sampler, uint pname, int* param) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glSamplerParameterIiv)(sampler, pname, param);
    
    private static nint pfn_glSamplerParameterIuiv;
    public static void glSamplerParameterIuiv(uint sampler, uint pname, uint* param) => ((delegate* unmanaged[Stdcall]<uint, uint, uint*, void>)pfn_glSamplerParameterIuiv)(sampler, pname, param);
    
    private static nint pfn_glGetSamplerParameterIiv;
    public static void glGetSamplerParameterIiv(uint sampler, uint pname, int* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, int*, void>)pfn_glGetSamplerParameterIiv)(sampler, pname, @params);
    
    private static nint pfn_glGetSamplerParameterIuiv;
    public static void glGetSamplerParameterIuiv(uint sampler, uint pname, uint* @params) => ((delegate* unmanaged[Stdcall]<uint, uint, uint*, void>)pfn_glGetSamplerParameterIuiv)(sampler, pname, @params);
    
    private static nint pfn_glTexBuffer;
    public static void glTexBuffer(uint target, uint internalformat, uint buffer) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, void>)pfn_glTexBuffer)(target, internalformat, buffer);
    
    private static nint pfn_glTexBufferRange;
    public static void glTexBufferRange(uint target, uint internalformat, uint buffer, int offset, int size) => ((delegate* unmanaged[Stdcall]<uint, uint, uint, int, int, void>)pfn_glTexBufferRange)(target, internalformat, buffer, offset, size);
    
    private static nint pfn_glTexStorage3DMultisample;
    public static void glTexStorage3DMultisample(uint target, int samples, uint internalformat, int width, int height, int depth, byte fixedsamplelocations) => ((delegate* unmanaged[Stdcall]<uint, int, uint, int, int, int, byte, void>)pfn_glTexStorage3DMultisample)(target, samples, internalformat, width, height, depth, fixedsamplelocations);
    
}
