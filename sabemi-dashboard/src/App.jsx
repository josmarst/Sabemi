import { useEffect, useState } from "react";
import api from "./services/api";

function App() {
    const [pagamentos, setPagamentos] = useState([]);
    const [statusFiltro, setStatusFiltro] = useState("");
    const [contratoFiltro, setContratoFiltro] = useState("");

    async function carregarPagamentos() {
        try {
            let url = "/pagamentos";

            const parametros = [];

            if (statusFiltro) {
                parametros.push(`status=${statusFiltro}`);
            }

            if (contratoFiltro) {
                parametros.push(`contrato=${contratoFiltro}`);
            }

            if (parametros.length > 0) {
                url += `?${parametros.join("&")}`;
            }

            const response = await api.get(url);

            setPagamentos(response.data);
        } catch (error) {
            console.error("Erro ao carregar pagamentos:", error);
        }
    }

    useEffect(() => {
        carregarPagamentos();

        const intervalo = setInterval(() => {
            carregarPagamentos();
        }, 5000);

        return () => clearInterval(intervalo);
    }, [statusFiltro, contratoFiltro]);

    return (
        <div className="container mt-4">
            <div className="card shadow p-4">

                <h2 className="mb-4">
                    Dashboard de Pagamentos
                </h2>

                <div className="row mb-4">

                    <div className="col-md-4">
                        <label className="form-label">
                            Status
                        </label>

                        <select
                            className="form-select"
                            value={statusFiltro}
                            onChange={(e) =>
                                setStatusFiltro(e.target.value)}
                        >
                            <option value="">
                                Todos
                            </option>

                            <option value="SUCESSO">
                                Sucesso
                            </option>

                            <option value="ERRO">
                                Erro
                            </option>

                        </select>
                    </div>

                    <div className="col-md-4">

                        <label className="form-label">
                            Contrato
                        </label>

                        <input
                            type="text"
                            className="form-control"
                            placeholder="CTR001"
                            value={contratoFiltro}
                            onChange={(e) =>
                                setContratoFiltro(e.target.value)}
                        />
                    </div>

                    <div className="col-md-4 d-flex align-items-end">

                        <button
                            className="btn btn-primary w-100"
                            onClick={carregarPagamentos}
                        >
                            Atualizar
                        </button>

                    </div>

                </div>

                <table className="table table-bordered table-hover">

                    <thead className="table-dark">

                        <tr>
                            <th>ID</th>
                            <th>Transação</th>
                            <th>Contrato</th>
                            <th>Valor</th>
                            <th>Status</th>
                            <th>Processado</th>
                            <th>Erro</th>
                        </tr>

                    </thead>

                    <tbody>

                        {pagamentos.length === 0 && (
                            <tr>
                                <td
                                    colSpan="7"
                                    className="text-center"
                                >
                                    Nenhum pagamento encontrado
                                </td>
                            </tr>
                        )}

                        {pagamentos.map((item) => (

                            <tr
                                key={item.id}
                                className={
                                    item.erro
                                        ? "table-danger"
                                        : ""
                                }
                            >

                                <td>{item.id}</td>

                                <td>{item.idTransacao}</td>

                                <td>{item.idContrato}</td>

                                <td>
                                    R$ {item.valor}
                                </td>

                                <td>

                                    <span
                                        className={
                                            item.status === "SUCESSO"
                                                ? "badge bg-success"
                                                : "badge bg-danger"
                                        }
                                    >
                                        {item.status}
                                    </span>

                                </td>

                                <td>
                                    {item.processado
                                        ? "Sim"
                                        : "Não"}
                                </td>

                                <td>
                                    {item.erro || "-"}
                                </td>

                            </tr>

                        ))}

                    </tbody>

                </table>

            </div>
        </div>
    );
}

export default App;