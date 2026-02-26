var beneficiarios = [];

$(document).ready(function () {
    $('#beneficiarioCPF').mask('000.000.000-00');

    $('#botaoBenef').click(function (e) {
        e.preventDefault();
        $('#modalBeneficiarios').modal('show');
    });

    $('#btnAdicionarBeneficiario').click(function () {
        adicionarBeneficiario();
    });
});

function adicionarBeneficiario() {
    var cpf = $('#beneficiarioCPF').val();
    var nome = $('#beneficiarioNome').val();

    if (!cpf || !nome) {
        ModalDialog('Atenção', 'Por favor, preencha CPF e Nome!');
        return;
    }

    beneficiarios.push({ Id: 0, CPF: cpf, Nome: nome, IDCliente: 0 });
    atualizarTabelaBeneficiarios();

    $('#beneficiarioCPF').val('');
    $('#beneficiarioNome').val('');
}

function atualizarTabelaBeneficiarios() {
    var tbody = $('#beneficiariosTableBody');
    tbody.empty();

    if (beneficiarios.length === 0) {
        tbody.append('<tr><td colspan="3" class="text-center text-muted">Nenhum beneficiário adicionado</td></tr>');
        return;
    }

    beneficiarios.forEach(function (b, index) {
        var row = '<tr>' +
            '<td>' + b.CPF + '</td>' +
            '<td>' + b.Nome + '</td>' +
            '<td style="padding: 5px; text-align: right;">' +
            '<button type="button" class="btn btn-sm btn-primary" onclick="editarBeneficiario(' + index + ')" style="margin-right: 5px;">Alterar</button>' +
            '<button type="button" class="btn btn-sm btn-primary" onclick="removerBeneficiario(' + index + ')">Excluir</button>' +
            '</td>' +
            '</tr>';
        tbody.append(row);
    });
}

function editarBeneficiario(index) {
    var b = beneficiarios[index];
    $('#beneficiarioCPF').val(b.CPF);
    $('#beneficiarioNome').val(b.Nome);

    beneficiarios.splice(index, 1);
    atualizarTabelaBeneficiarios();
}

function removerBeneficiario(index) {
    if (confirm('Deseja remover este beneficiário?')) {
        beneficiarios.splice(index, 1);
        atualizarTabelaBeneficiarios();
    }
}

function carregarBeneficiarios(idCliente) {
    $.ajax({
        url: '/Beneficiario/Listar',
        method: 'GET',
        data: { idCliente: idCliente },
        success: function (r) {
            if (r.Result === 'OK' && r.Records) {
                beneficiarios = r.Records.map(function(b) {
                    return {
                        Id: b.Id,
                        CPF: formatarCPF(b.CPF),
                        Nome: b.Nome,
                        IDCliente: b.IdCliente
                    };
                });
                atualizarTabelaBeneficiarios();
            }
        },
        error: function (r) {
            console.error('Erro ao carregar beneficiários:', r);
        }
    });
}

function formatarCPF(cpf) {
    if (!cpf) return '';
    cpf = cpf.replace(/\D/g, '');
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
}
