
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { Address, CreateAddressRequest } from '../models/address/address';


@Injectable({ providedIn: 'root' })
export class AddressService {
    private http = inject(HttpClient);
    private url = `${environment.apiUrl}/Address`;

    getAll() { return this.http.get<Address[]>(this.url); }
    getById(id: number) { return this.http.get<Address>(`${this.url}/${id}`); }
    create(dto: CreateAddressRequest) {
        return this.http.post<Address>(this.url, dto);
    }
    update(id: number, dto: CreateAddressRequest) {
        return this.http.put(`${this.url}/${id}`, dto);
    }
    delete(id: number) {
        return this.http.delete(`${this.url}/${id}`);
    }
}
