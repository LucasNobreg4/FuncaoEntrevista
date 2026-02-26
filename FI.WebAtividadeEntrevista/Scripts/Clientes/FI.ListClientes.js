$(document).ready(function () {

    if (document.getElementById("gridClientes"))
        $('#gridClientes').jtable({
            title: 'Clientes',
            paging: true, //Enable paging
            pageSize: 5, //Set page size (default: 10)
            sorting: true, //Enable sorting
            defaultSorting: 'Nome ASC', //Set default sorting
            actions: {
                listAction: urlClienteList,
            },
            fields: {
                Nome: {
                    title: 'Nome',
                    width: '50%'
                },
                Email: {
                    title: 'Email',
                    width: '35%'
                },
                CPF: {
                    title: 'CPF',
                    width: '15%',
                    display: function (data) {
                        var cpf = data.record.CPF || '';
                        // Remove caracteres não numéricos
                        cpf = cpf.replace(/\D/g, '');
                        // Formata CPF: 000.000.000-00
                        if (cpf.length === 11) {
                            return cpf.substring(0, 3) + '.' + 
                                   cpf.substring(3, 6) + '.' + 
                                   cpf.substring(6, 9) + '-' + 
                                   cpf.substring(9, 11);
                        }
                        return cpf;
                    }
                },
                Alterar: {
                    title: '',
                    display: function (data) {
                        return '<button onclick="window.location.href=\'' + urlAlteracao + '/' + data.record.Id + '\'" class="btn btn-primary btn-sm">Alterar</button>';
                    }
                },

            }
        });

    //Load student list from server
    if (document.getElementById("gridClientes"))
        $('#gridClientes').jtable('load');
})