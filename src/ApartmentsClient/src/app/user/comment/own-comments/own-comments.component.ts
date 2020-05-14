import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CommentUserService, PagedResponseOfCommentDTO } from 'src/app/services/nswag.generated.service';

@Component({
  selector: 'app-own-comments',
  templateUrl: './own-comments.component.html',
  styleUrls: ['./own-comments.component.scss']
})
export class OwnCommentsComponent implements OnInit {

  response: PagedResponseOfCommentDTO;
  requestForm: FormGroup;
  pages: number[] = [];

  constructor(
    private commentService: CommentUserService,
    private fb: FormBuilder
  ) { 
    this.requestForm = this.fb.group({
      pageNumber: 1,
      pageSize: 20
    });
  }

  ngOnInit(): void {
  }

  getOwnComments(){
    this.commentService.getAllCommentsByAuthorId(this.requestForm.value)
    .subscribe(response => {
      this.response = response;
    
      this.pages = [];
      for (let index = 1; index <= response.totalPages; index++) {
        this.pages.push(index);
      }
    });
  }

  previousePage(){
    if (this.response != null && this.response.hasPreviousPage) {
      this.requestForm.value.pageNumber -= 1;
      this.getOwnComments();
    }
  }

  nextPage(){
    if (this.response != null && this.response.hasNextPage) {
      this.requestForm.value.pageNumber += 1;
      this.getOwnComments();
    }
  }

  getPage(page: number){
    if (this.response != null && page > 0 && page <= this.response.totalPages) {
      this.requestForm.value.pageNumber = page;
      this.getOwnComments();
    }
  }

}
