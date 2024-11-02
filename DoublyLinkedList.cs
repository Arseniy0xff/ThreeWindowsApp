using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeWindowsApp {
    internal class Node {
        public double Data;
        public double Weight;
        public double A;
        public Node Next;
        public Node Prev;

        public Node(double data, double weight=0.5) {
            Data = data;
            Weight = weight;
            Next = null;
            Prev = null;
        }
    }

    internal class DoublyLinkedList {
        private Node head;

        public DoublyLinkedList() {
            head = null;
        }

        public void AddToEnd(double data) {
            Node newNode = new Node(data);
            if (head == null) {
                head = newNode;
            } else {
                Node temp = head;
                while (temp.Next != null) {
                    temp = temp.Next;
                }
                temp.Next = newNode;
                newNode.Prev = temp;
            }
        }

        public void PrintList() {
            Node temp = head;
            while (temp != null) {
                Debug.Write(temp.Data + " ");
                temp = temp.Next;
            }
            Debug.Write("\n");
           
        }

        public void AddToStart(double data) {
            Node newNode = new Node(data);
            if (head == null) {
                head = newNode;
            } else {
                newNode.Next = head;
                head.Prev = newNode;
                head = newNode;
            }
        }

        public double GetValueAt(int index) {
            Node temp = head;
            int currentIndex = 0;

            while (temp != null) {
                if (currentIndex == index) {
                    return temp.Data;
                }
                temp = temp.Next;
                currentIndex++;
            }

            throw new IndexOutOfRangeException("Index out of range");
        }

        public void SetValueAt(int index, double newValue) {
            Node temp = head;
            int currentIndex = 0;

            while (temp != null) {
                if (currentIndex == index) {
                    temp.Data = newValue; // Перезаписываем значение
                    return;
                }
                temp = temp.Next;
                currentIndex++;
            }

            throw new IndexOutOfRangeException("Index out of range");
        }


        public double GetPreviousValue(int index) {
            Node temp = head;
            int currentIndex = 0;

            while (temp != null) {
                if (currentIndex == index) {
                    if (temp.Prev != null) {
                        return temp.Prev.Data;
                    } else {
                        throw new InvalidOperationException("No previous node exists.");
                    }
                }
                temp = temp.Next;
                currentIndex++;
            }

            throw new IndexOutOfRangeException("Index out of range");
        }

        public double GetNextValue(int index) {
            Node temp = head;
            int currentIndex = 0;

            while (temp != null) {
                if (currentIndex == index) {
                    if (temp.Next != null) {
                        return temp.Next.Data;
                    } else {
                        throw new InvalidOperationException("No next node exists.");
                    }
                }
                temp = temp.Next;
                currentIndex++;
            }

            throw new IndexOutOfRangeException("Index out of range");
        }

    }
}
