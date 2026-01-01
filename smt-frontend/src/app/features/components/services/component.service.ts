import { Injectable } from "@angular/core";
import { ApiService } from "../../../core/services/api.service";
import { Component } from "../../../shared/models/component.model";

@Injectable({
  providedIn: 'root'
})
export class ComponentService {
  constructor(private api: ApiService) {}

  getComponents() 
  {
    return this.api.get<Component[]>("components");
  }
}