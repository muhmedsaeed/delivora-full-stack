
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { Category } from '../models/category/category';


@Injectable({ providedIn: 'root' })
export class CategoryService {
    private http = inject(HttpClient);
    private url = `${environment.apiUrl}/Category`;

    getAll() { return this.http.get<Category[]>(this.url); }
    getById(id: number) { return this.http.get<Category>(`${this.url}/${id}`); }
    getByName(name: string) { return this.http.get<Category>(`${this.url}/${name}`); }

    create(formData: FormData) {
        return this.http.post<Category>(this.url, formData);
    }
    update(id: number, formData: FormData) {
        return this.http.put(`${this.url}/${id}`, formData);
    }
    delete(id: number) {
        return this.http.delete(`${this.url}/${id}`);
    }

}
