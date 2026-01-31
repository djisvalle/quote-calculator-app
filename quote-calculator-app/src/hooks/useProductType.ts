import { useQuery } from "@tanstack/react-query";
import { productTypeApi } from "../infrastracture/productTypeApi";

export function useGetAllProductTypes() {
    return useQuery({
        queryKey: ['product-types'],
        queryFn: productTypeApi.getAll,
        initialData: []
    })
}