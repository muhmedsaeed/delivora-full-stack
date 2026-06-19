
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { Food } from '../models/food/food';

@Injectable({ providedIn: 'root' })
export class FoodService {
    private http = inject(HttpClient);
    private url = `${environment.apiUrl}/Food`;
    getAll() { return this.http.get<Food[]>(this.url); }
    getById(id: number) { return this.http.get<Food>(`${this.url}/${id}`); }
    getByCategoryId(categoryId: number) {
        return this.http.get<Food[]>(`${this.url}/bycategory/${categoryId}`);
    }
    getByCategoryName(name: string) {
        return this.http.get<Food[]>(`${this.url}/bycategory/${name}`);
    }

    create(formData: FormData) {
        return this.http.post<Food>(this.url, formData);
    }
    update(id: number, formData: FormData) {
        return this.http.put<Food>(`${this.url}/${id}`, formData);
    }
    delete(id: number) {
        return this.http.delete(`${this.url}/${id}`);
    }
}