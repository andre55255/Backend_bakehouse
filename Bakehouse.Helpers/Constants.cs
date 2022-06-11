namespace Bakehouse.Helpers
{
    public static class ConstantsMessageBaseRepository
    {
        // Error
        public static string ErrorInsert = "Erro ao inserir um registro na tabela: ";
        public static string ErrorEdit = "Erro ao editar um registro na tabela: ";
        public static string ErrorFindAll = "Erro ao buscar dados na tabela: ";
        public static string ErrorFindById = "Erro ao buscar registro por id na tabela: ";
        public static string ErrorFindByToken = "Erro ao buscar registro por campo na tabela: ";
        public static string ErrorDisabled = "Erro ao desabilitar um registro na tabela: ";
    }

    public static class ConstantsEmail
    {
        // Error
        public static string ErrorLoadTemplate = "Erro ao carregar template de email";
        public static string ErrorSendMail = "Erro ao enviar email";
    }

    public static class ConstantsMessageConfiguration
    {
        // Error
        public static string ErrorFindAll = "Erro ao buscar lista de configurações no banco de dados";
        public static string ErrorFindByToken = "Erro ao buscar configuração por token no banco de dados";
        public static string ErrorFindById = "Erro ao buscar configuração por id no banco de dados";
        public static string ErrorInsert = "Erro ao inserir nova configuração no banco de dados";
        public static string ErrorSave = "Erro ao salvar configuração";
        public static string ErrorConfigTokenExists = "Erro ao inserir nova configuração, já existe uma config com este token";
        public static string ErrorConfigNotFound = "Erro, não encontrada uma configuração com este token";
        public static string ErrorUpdate = "Erro ao atualizar configuração no banco de dados";
        public static string ErrorDelete = "Erro ao desabilitar configuração no banco de dados";
        public static string ErrorTypeDbEmpty = "Não há nenhuma configuração registrada no banco de dados";
        public static string ErrorPrepare = "Erro ao preparar tela de configuração";

        // Success
        public static string SuccessInsert = "Configuração salva com sucesso";
        public static string SuccessUpdate = "Configuração atualizada com sucesso";
        public static string SuccessDelete = "Configuração deletada com sucesso";
    }

    public static class ConstantsMessageUsers
    {
        // Error
        public static string ErrorCreateUser = "Erro ao realizar criação de usuário";
        public static string ErrorCreateUserDB = "Erro ao criar usuário identity no banco de dados";
        public static string ErrorAddRoleToUser = "Erro ao adicionar role para usuário: ";
        public static string ErrorUserEmailExists = "Erro, email já existe na base de dados";
        public static string ErrorGetById = "Erro ao buscar usuário por id";
        public static string ErrorActiveUser = "Erro ao realizar confirmação de conta por email de usuario";
        public static string ErrorSendEmail = "Erro ao enviar email";
        public static string ErrorLogInto = "Erro ao fazer login";
        public static string ErrorFindUserByEmail = "Erro ao recuperar usuário por email";
        public static string ErrorFindRoleByUser = "Erro ao recuperar roles de usuário";
        public static string ErrorFindAllRoles = "Erro ao recuperar todas as roles do banco";
        public static string ErrorRemoveRolesByUser = "Erro ao deletar roles do usuário";
        public static string ErrorGenerateTokenResetPassword = "Erro ao gerar token de redefinição de senha";
        public static string ErrorGenerateTokenConfirmEmail = "Erro ao gerar token de confirmacao de conta";
        public static string ErrorResetPassword = "Erro ao redefinir senha";
        public static string ErrorGetAll = "Erro ao listar usuários por perfil";
        public static string ErrorGetAllUsers = "Erro ao listar todos os usuários";
        public static string ErrorUserNotFound = "Erro, usuário não encontrado";
        public static string ErrorCheckEmailConfirmed = "Erro ao checar se email está confirmado";
        public static string ErrorUserNotConfirmed = "Usuário não está com o email confirmado";
        public static string ErrorErrorConfirmEmail = "Erro ao confirmar email do usuário";
        public static string ErrorErrorConfirmEmailCode = "Erro ao confirmar email do usuário, código inválido";
        public static string ErrorGenerateTokenJwt = "Erro ao gerar token jwt do usuário: ";
        public static string ErrorGenerateRefreshToken = "Erro ao gerar código de refresh token do usuário: ";
        public static string ErrorRefreshToken = "Erro ao atualizar o token jwt";
        public static string ErrorGetAuthToken = "Erro ao recuperar token de autenticacao";
        public static string ErrorUpdateUser = "Erro ao atualizar informação de usuário";
        public static string ErrorUpdateUserDB = "Erro ao atualizar informação de usuário no banco de dados";
        public static string ErrorDisableUser = "Erro ao deletar usuário";
        public static string ErrorDisableUserDB = "Erro ao desabilitar usuário no banco de dados";
        public static string FailedResetPassword = "Falha ao recuperar a senha";
        public static string ErrorPasswordInvalid = "Senha não informada ou inválida, deve ter entre 6 e 20 caracteres";
        public static string ErrorPrepare = "Erro ao preparar dados de usuário";

        // Success
        public static string SuccessSendMailResetPassword = "Email de recuperação de senha enviado com sucesso";
        public static string SuccessResetPassword = "Senha recuperada com sucesso";
        public static string SuccessLogin = "Login efetuado com sucesso";
        public static string SuccessCreated = "Usuário criado com sucesso, email para confirmação de conta enviado";
        public static string SuccessUpdated = "Usuário editado com sucesso";
        public static string SuccessDisabled = "Usuário desabilitado com sucesso";
        public static string SuccessConfirmAccountEmail = "Email confirmado com sucesso, faça seu login";
    }
}