import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { Profile } from '../models/profile/profile';

@Injectable({ providedIn: 'root' })
export class ProfileService {
    private http = inject(HttpClient);
    private url = `${environment.apiUrl}/Profile`;

    get() {
        return this.http.get<Profile>(this.url);
    }

    update(formData: FormData) {
        return this.http.put<Profile>(this.url, formData);
    }

    deleteImage() {
        return this.http.delete(`${this.url}/image`);
    }

    deactivate() {
        return this.http.delete(this.url);
    }
}
