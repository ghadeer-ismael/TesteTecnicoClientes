using System;

namespace Clientes.Domain.Shared
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }

        public BaseResponse()
        {
            // Sucesso se não deu erros
            Success = true;
        }

        public BaseResponse(Exception ex)
        {
            Success = false;
            Message = ex.Message ?? "Ocorreu um erro";
            Errors = GetErros(ex)?.Split(";");
        }

        public string GetErros(Exception ex)
        {
            var mensagem = ex.Message.ToLower().Contains("inner exception") || ex.Message.ToLower().Contains("one or more errors occurred") ?
                        (ex.InnerException.Message.ToLower().Contains("inner exception") || ex.InnerException.Message.ToLower().Contains("an error occurred while sending the request") ?
                        ex.InnerException.InnerException.Message : ex.InnerException.Message)
                        : ex.Message;

            return mensagem;
        }
    }

    public class BaseResponse<T> : BaseResponse where T : class
    {
        public T Data { get; set; }

        public BaseResponse() : base() { }
        public BaseResponse(Exception ex) : base(ex) { }

        public BaseResponse(T data)
        {
            // Sucesso se não deu erros
            Success = true;
            Data = data;
        }

    }
}
