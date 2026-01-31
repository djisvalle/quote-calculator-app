import { useQuery, useMutation } from "@tanstack/react-query";
import { loanApi } from "../infrastracture/loanApi";
import type { CalculateLoanRequest, CalculateLoanDetails } from "@/types/LoanApplication";
import type { UseQueryOptions } from "@tanstack/react-query";

export function useGetDetailsByPublicId(publicId: string | null) {
    return useQuery({
        queryKey: ['loan', publicId],
        queryFn: () => loanApi.getDetailsByPublicId(publicId),
        enabled: !!publicId,
        staleTime: 0,
        refetchOnWindowFocus: false,   // don't refetch when window gains focus
        refetchOnReconnect: false,     // don't refetch when reconnecting
    });
}

export function useSaveDraftLoanApplication() {
    return useMutation({
        mutationFn: loanApi.saveDraftLoanApplication
    })
}

export function useCalculateLoanApplication() {
    return useMutation({
        mutationFn: loanApi.calculateLoanApplication
    })
}

// export function useCalculateLoanApplication(params: Partial<CalculateLoanRequest>) {
//     return useQuery({
//         queryKey: ['calculate-loan', params.productTypeId, params.amount, params.term], 
//         queryFn: () => loanApi.calculateLoanApplication(params as CalculateLoanRequest),
//         enabled: !!(params.productTypeId && params.amount && params.term),
//         staleTime: 1000 * 60 * 5,
//     });
// }

export function useFinalizeLoanApplication() {
    return useMutation({
        mutationFn: loanApi.finalizeLoanApplication
    })
}

export function useReceiptDetailsByPublicId(publicId: string | null) {
    return useQuery({
        queryKey: ['receipt', publicId],
        queryFn: () => loanApi.getReceiptDetailsByPublicId(publicId),
        enabled: !!publicId,
        staleTime: Infinity,
        refetchOnWindowFocus: false,   // don't refetch when window gains focus
        refetchOnReconnect: false,     // don't refetch when reconnecting
    });
}
