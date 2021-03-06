import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CommentAdministrationService,
  PagedResponseOfCommentDTOAdministration
 } from 'src/app/services/nswag.generated.service'


@Component({
  selector: 'app-comments-administration',
  templateUrl: './comments-administration.component.html',
  styleUrls: ['./comments-administration.component.scss']
})
export class CommentsAdministrationComponent implements OnInit {

  errorMessage: string;

  response: PagedResponseOfCommentDTOAdministration;
  requestForm: FormGroup;
  pages: number[] = [];

  constructor(
    private commentService: CommentAdministrationService,
    private fb: FormBuilder,
    private route: ActivatedRoute
    ) { 
      this.requestForm = this.fb.group({
        data: this.getUserId(),
        pageNumber: 1,
        pageSize: 20
      });
  }

  ngOnInit(): void {
  }

  getUserId(){
    return this.route.snapshot.paramMap.get('id');
  }

  getComments(){
    this.errorMessage = null;

    this.commentService.getAllCommentsByUserId(this.requestForm.value)
    .subscribe(response => {
      this.response = response;
    
      this.pages = [];
      for (let index = 1; index <= response.totalPages; index++) {
        this.pages.push(index);
      }
    },
    error=>{
 
      if (error.status ===  500) {
        this.errorMessage = "Error 500: Internal Server Error";
      }
      if (error.status ===  400) {
        this.errorMessage = "Error 400: " + error.response;
      }
      if (error.status ===  403) {
        this.errorMessage = "Error 403: You are not authorized";
      }
      else{
        this.errorMessage = "An error occurred.";
      }
    })
  }

  previousePage(){
    if (this.response != null && this.response.hasPreviousPage) {
      this.requestForm.value.pageNumber -= 1;
      this.getComments();
    }
  }

  nextPage(){
    if (this.response != null && this.response.hasNextPage) {
      this.requestForm.value.pageNumber += 1;
      this.getComments();
    }
  }

  getPage(page: number){
    if (this.response != null && page > 0 && page <= this.response.totalPages) {
      this.requestForm.value.pageNumber = page;
      this.getComments();
    }
  }

}
