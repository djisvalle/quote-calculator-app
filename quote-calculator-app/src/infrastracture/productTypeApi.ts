import { http } from "./http.ts";
import type { ProductType } from "../types/ProductType";

export const productTypeApi = {
    getAll: async() => {
        const res = await http.get<ProductType[]>('/producttype');
        return res.data
    }
}